using Application.Interfaces;
using Application.Interfaces.Infrastructure.MQTT;
using Core.Domain.Entities;
using Microsoft.Extensions.Logging;
using WebSocketBoilerplate;
using Fleck;

namespace Api.Websocket;


public class WaterNowClientDto : BaseDto
{
    public string PlantId { get; set; }
}

public class WaterNowServerResponse : BaseDto
{
    public string Message { get; set; }
}

public class WaterNowEventHandler : BaseEventHandler<WaterNowClientDto>
{
    private readonly IWateringService _wateringService;
    private readonly ILogger<WaterNowEventHandler> _logger;

    public WaterNowEventHandler(IWateringService wateringService, ILogger<WaterNowEventHandler> logger)
    {
        _wateringService = wateringService;
        _logger = logger;
    }

    public override async Task Handle(WaterNowClientDto dto, IWebSocketConnection socket)
    {
        _logger.LogInformation("Received WaterNow request for plant {PlantId}", dto.PlantId);

        // Trigger the actual watering via MQTT
        await _wateringService.TriggerWateringAsync(dto.PlantId);

        // Respond to the client (optional)
        socket.SendDto(new WaterNowServerResponse
        {
            requestId = dto.requestId,
            Message = $"Watering triggered for plant {dto.PlantId}"
        });
    }
    
}