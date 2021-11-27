using System.ComponentModel.DataAnnotations;

namespace MyServiceBroker;

public class ServiceInstanceEntity
{
    [Key]
    public string Id { get; set; } = default!;

    public string ServiceId { get; set; } = default!;

    public string PlanId { get; set; } = default!;

    public string? Parameters { get; set; }
}
