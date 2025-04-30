using Application.Interfaces.Infrastructure.MQTT;
using Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class WateringService : IWateringService
{
    private readonly IMqttPublisher _mqttPublisher;
    private readonly ILogger<WateringService> _logger;

    public WateringService(IMqttPublisher mqttPublisher, ILogger<WateringService> logger)
    {
        _mqttPublisher = mqttPublisher;
        _logger = logger;
    }

    public async Task TriggerWateringAsync(Guid plantId)
    {
        const string wateringTopic = "plants/water";

        var payload = new
        {
            plantId = plantId.ToString(),
            command = "ON"
        };

        _logger.LogInformation("Triggering watering for Plant {PlantId}", plantId);

        await _mqttPublisher.Publish(payload, wateringTopic);
    
    }
}