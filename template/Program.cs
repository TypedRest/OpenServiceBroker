using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyServiceBroker;
using Newtonsoft.Json;
using OpenServiceBroker;
using OpenServiceBroker.Catalogs;
using OpenServiceBroker.Instances;

var builder = WebApplication.CreateBuilder(args);

// Catalog of available services
string catalogPath = File.ReadAllText(Path.Combine(ApplicationEnvironment.ApplicationBasePath, "catalog.json"));
builder.Services.AddSingleton(JsonConvert.DeserializeObject<Catalog>(catalogPath)!);

// Database for storing provisioned service instances
builder.Services.AddDbContext<MyServiceBroker.DbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Database")));

// Implementations of OpenServiceBroker.Server interfaces
builder.Services.AddTransient<ICatalogService, CatalogService>()
                .AddTransient<IServiceInstanceBlocking, ServiceInstanceService>();

// Open Service Broker REST API
builder.Services.AddControllers()
                .AddOpenServiceBroker();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My Service Broker",
        Version = "v1"
    });
}).AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
