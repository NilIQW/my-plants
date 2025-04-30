namespace Application.Interfaces.Infrastructure.MQTT;

public interface IWateringService
{
    Task TriggerWateringAsync(Guid plantId);

}