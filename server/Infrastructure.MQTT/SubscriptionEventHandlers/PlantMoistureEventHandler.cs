using Application.Interfaces.Infrastructure.MQTT;
using Application.Interfaces.Infrastructure.Postgres;
using Application.Models.Dtos.RestDtos.PlantDtos;
using Application.Services;
using HiveMQtt.Client.Events;
using HiveMQtt.MQTT5.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.MQTT.SubscriptionEventHandlers;

public class PlantMoistureMessageHandler : IMqttMessageHandler
{
    public string TopicFilter => "plants/moisture";
    public QualityOfService QoS => QualityOfService.AtMostOnceDelivery;

    private readonly ILogger<PlantMoistureMessageHandler> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PlantMoistureMessageHandler(
        ILogger<PlantMoistureMessageHandler> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

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

            if (moistureData == null)
            {
                _logger.LogWarning("Received payload could not be deserialized into PlantMoistureData.");
                return;
            }

            _logger.LogInformation($"Received moisture data: plantId = {moistureData.PlantId}, moisture = {moistureData.Moisture}");

            using var scope = _serviceScopeFactory.CreateScope();
            var plantService = scope.ServiceProvider.GetRequiredService<IPlantService>();
            var wateringService = scope.ServiceProvider.GetRequiredService<IWateringService>();

            var plant = await plantService.GetByIdAsync(moistureData.PlantId);

            if (plant == null)
            {
                _logger.LogWarning($"Plant with ID {moistureData.PlantId} not found in the database.");
                return;
            }

            // Update the moisture value regardless
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

            _logger.LogInformation($"Updated plant moisture level in DB: {plantDto.MoistureLevel}");

            // If moisture is below threshold, wait and recheck
            if (moistureData.Moisture > plant.MoistureThreshold && plant.IsAutoWateringEnabled)
            {
                _logger.LogInformation($"Moisture below threshold for Plant {plant.Id}. Waiting 5 seconds to recheck...");

                await Task.Delay(5000); // Wait 5 seconds

                // Re-fetch the plant data from DB (assuming ESP32 updates the value again)
                var recheckedPlant = await plantService.GetByIdAsync(plant.Id);

                if (recheckedPlant.MoistureLevel > plant.MoistureThreshold)
                {
                    _logger.LogInformation($"Moisture still low after recheck. Triggering watering for Plant {plant.Id}.");

                    await wateringService.TriggerWateringAsync(plant.Id);
                    
                    await Task.Delay(10000); 


                    _logger.LogInformation($"Watering triggered for Plant {plant.Id}.");
                }
                else
                {
                    _logger.LogInformation($"Moisture rechecked and is now sufficient. No watering needed.");
                }
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
