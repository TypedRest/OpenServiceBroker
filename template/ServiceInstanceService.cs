using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenServiceBroker.Catalogs;
using OpenServiceBroker.Errors;
using OpenServiceBroker.Instances;

namespace MyServiceBroker
{
    public class ServiceInstanceService : IServiceInstanceBlocking
    {
        private readonly DbContext _context;
        private readonly ILogger<ServiceInstanceService> _logger;
        private readonly Catalog _catalog;

        public ServiceInstanceService(DbContext context, Catalog catalog, ILogger<ServiceInstanceService> logger)
        {
            _context = context;
            _logger = logger;
            _catalog = catalog;
        }

        public async Task<ServiceInstanceResource> FetchAsync(string instanceId)
        {
            _logger.LogTrace("Read instance {InstanceId}", instanceId);

            var entity = await _context.ServiceInstances.FindAsync(instanceId);
            if (entity == null) throw new NotFoundException($"Instance '{instanceId}' not found.");

            return new ServiceInstanceResource
            {
                ServiceId = entity.ServiceId,
                PlanId = entity.PlanId,
                Parameters = (entity.Parameters == null) ? null : JsonConvert.DeserializeObject<JObject>(entity.Parameters)
            };
        }

        public async Task<ServiceInstanceProvision> ProvisionAsync(ServiceInstanceContext context, ServiceInstanceProvisionRequest request)
        {
            _logger.LogInformation("Provisioning instance {InstanceId} as service {ServiceId}", context.InstanceId, request.ServiceId);

            if (GetService(request.ServiceId).Plans.All(x => x.Id != request.PlanId))
                throw new BadRequestException($"Unknown plan ID '{request.PlanId}'.");

            var entity = await _context.ServiceInstances.FindAsync(context.InstanceId);
            if (entity != null)
            {
                if (entity.ServiceId == request.ServiceId
                 && entity.PlanId == request.PlanId
                 && JsonConvert.SerializeObject(request.Parameters) == (entity.Parameters ?? "null"))
                    return new ServiceInstanceProvision {Unchanged = true};
                else
                    throw new ConflictException($"There is already an instance {context.InstanceId} with different settings.");
            }

            await _context.ServiceInstances.AddAsync(new ServiceInstanceEntity
            {
                Id = context.InstanceId,
                ServiceId = request.ServiceId,
                PlanId = request.PlanId,
                Parameters = JsonConvert.SerializeObject(request.Parameters)
            });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ConflictException(ex.InnerException?.Message ?? ex.Message);
            }

            return new ServiceInstanceProvision();
        }

        private Service GetService(string id)
        {
            var service = _catalog.Services.FirstOrDefault(x => x.Id == id);
            if (service == null)
                throw new BadRequestException($"Unknown service ID '{id}'.");
            return service;
        }

        public async Task UpdateAsync(ServiceInstanceContext context, ServiceInstanceUpdateRequest request)
        {
            _logger.LogInformation("Updating instance {InstanceId} as service {ServiceId}", context.InstanceId, request.ServiceId);

            var entity = await _context.ServiceInstances.FindAsync(context.InstanceId);
            if (entity == null) throw new NotFoundException($"Instance '{context.InstanceId}' not found.");

            if (request.ServiceId != entity.ServiceId)
                throw new BadRequestException($"Cannot change service ID of instance '{context.InstanceId}' from '{entity.ServiceId}' to '{request.ServiceId}'.");
            if (request.PlanId != null && request.PlanId != entity.PlanId)
            {
                var service = GetService(request.ServiceId);
                if (!service.PlanUpdateable)
                    throw new BadRequestException($"Service ID '{request.ServiceId}' does not allow changing the Plan ID.");
                if (service.Plans.All(x => x.Id != request.PlanId))
                    throw new BadRequestException($"Unknown plan ID '{request.PlanId}'.");
                entity.PlanId = request.PlanId;
            }
            if (request.Parameters != null)
                entity.Parameters = JsonConvert.SerializeObject(request.Parameters);

            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeprovisionAsync(ServiceInstanceContext context, string serviceId, string planId)
        {
            _logger.LogInformation("Deprovisioning instance {InstanceId}", context.InstanceId);

            var entity = await _context.ServiceInstances.FindAsync(context.InstanceId);
            if (entity == null)
                throw new GoneException($"Instance '{context.InstanceId}' not found.");
            if (entity.ServiceId != serviceId || entity.PlanId != planId)
                throw new BadRequestException($"Service and/or plan ID for instance '{context.InstanceId}' do not match.");

            _context.ServiceInstances.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
