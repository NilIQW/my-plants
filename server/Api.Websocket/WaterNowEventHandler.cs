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
        try
        {
            _logger.LogInformation("Received WaterNow request for plant {PlantId}", dto.PlantId);

            await _wateringService.TriggerWateringAsync(dto.PlantId);

            socket.SendDto(new WaterNowServerResponse
            {
                requestId = dto.requestId,
                Message = $"Watering triggered for plant"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the WaterNow request for plant {PlantId}", dto.PlantId);

            socket.SendDto(new ServerSendsErrorMessage
            {
                requestId = dto.requestId,
                Message = $"An error occurred: {ex.Message}"
            });
        }
    }
    
}