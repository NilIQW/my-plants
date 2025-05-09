using WebSocketBoilerplate;

namespace Api.Websocket;

public class PlantMoistureDto : BaseDto
{
    public string PlantId { get; set; } = default!;
    public int MoistureLevel { get; set; }
}