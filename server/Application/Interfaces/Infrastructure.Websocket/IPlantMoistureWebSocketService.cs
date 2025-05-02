namespace Application.Interfaces.Infrastructure.Websocket;

public interface IPlantMoistureWebSocketService
{
    Task BroadcastMoistureUpdateAsync(Guid plantId, int moistureLevel);

}