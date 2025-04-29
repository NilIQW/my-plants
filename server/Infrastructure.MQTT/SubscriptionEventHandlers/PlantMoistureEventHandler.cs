using Application.Interfaces.Infrastructure.Postgres;
using Application.Models.Dtos.RestDtos.PlantDtos;
using Application.Services;
using HiveMQtt.Client.Events;
using HiveMQtt.MQTT5.Types;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MQTT.SubscriptionEventHandlers;

public class PlantMoistureMessageHandler : IMqttMessageHandler
{
    // Set the topic filter and Quality of Service (QoS) for this handler
    public string TopicFilter => "plants/moisture";
    public QualityOfService QoS => QualityOfService.AtMostOnceDelivery; 

    private readonly ILogger<PlantMoistureMessageHandler> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    
    // Inject logger to log any issues
    public PlantMoistureMessageHandler(
        ILogger<PlantMoistureMessageHandler> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    // Handle the incoming message
    public void Handle(object? sender, OnMessageReceivedEventArgs args)
    {
        _ = HandleInternalAsync(sender, args);
    }

    public async Task HandleInternalAsync(object? sender, OnMessageReceivedEventArgs args)
    {
        try
        {
            var payload = args.PublishMessage.PayloadAsString;
            var moistureData = JsonConvert.DeserializeObject<PlantMoistureData>(payload);

            if (moistureData != null)
            {
                _logger.LogInformation($"Received moisture data: plantId = {moistureData.PlantId}, moisture = {moistureData.Moisture}");

                // 🛠 Create a fresh scope for each message
                using var scope = _serviceScopeFactory.CreateScope();
                var plantService = scope.ServiceProvider.GetRequiredService<IPlantService>();

                var plant = await plantService.GetByIdAsync(moistureData.PlantId);

                if (plant == null)
                {
                    _logger.LogWarning($"Plant with ID {moistureData.PlantId} not found in the database.");
                    return;
                }

                var plantDto = new PlantDto
                {
                    Id = plant.Id,
                    PlantName = plant.PlantName,
                    PlantType = plant.PlantType,
                    MoistureLevel = moistureData.Moisture,
                    MoistureThreshold = plant.MoistureThreshold,
                    IsAutoWateringEnabled = plant.IsAutoWateringEnabled
                };
                await plantService.UpdateAsync(plantDto.Id, new UpdatePlantDto
                {
                       PlantName = plantDto.PlantName,
                       PlantType = plantDto.PlantType,
                       MoistureLevel = plantDto.MoistureLevel,
                       MoistureThreshold = plantDto.MoistureThreshold,
                       IsAutoWateringEnabled = plantDto.IsAutoWateringEnabled
                });

                _logger.LogInformation($"Mapped PlantDto: {JsonConvert.SerializeObject(plantDto)}");
            }
            else
            {
                _logger.LogWarning("Received payload could not be deserialized into PlantMoistureData.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing incoming moisture data message.");
        }
    }

}

public class PlantMoistureData
{
    public Guid PlantId { get; set; }
    public int Moisture { get; set; }
}
