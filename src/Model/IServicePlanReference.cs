namespace OpenServiceBroker;

public interface IServicePlanReference
{
    string ServiceId { get; set; }

    string PlanId { get; set; }
}
