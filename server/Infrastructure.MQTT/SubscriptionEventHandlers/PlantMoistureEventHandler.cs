using Application.Interfaces;
using Application.Interfaces.Infrastructure.MQTT;
using Application.Interfaces.Infrastructure.Websocket;
using Application.Interfaces.Infrastructure.Postgres;
using Application.Models.Dtos.MqttSubscriptionDto;
using Application.Models.Dtos.RestDtos.PlantDtos;
using Application.Models.Dtos.RestDtos.WateringLogDtos;
using Application.Models.Enums;
using Application.Services;
using HiveMQtt.Client.Events;
using HiveMQtt.MQTT5.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.MQTT.SubscriptionEventHandlers
{
    public class PlantMoistureMessageHandler : IMqttMessageHandler
    {
        public string TopicFilter => "plants/moisture";
        public QualityOfService QoS => QualityOfService.AtMostOnceDelivery;

        private readonly ILogger<PlantMoistureMessageHandler> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnectionManager _connectionManager;

        public PlantMoistureMessageHandler(
            IConnectionManager connectionManager,
            ILogger<PlantMoistureMessageHandler> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _connectionManager = connectionManager;
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
                var wateringLogService = scope.ServiceProvider.GetRequiredService<IWateringLogService>();

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

                _logger.LogInformation($"Updated plant moisture level in DB: {plantDto.MoistureLevel}");
                await _connectionManager.BroadcastToTopic(nameof(PlantDto), JsonConvert.SerializeObject(new WrapperForDto() { dto = plantDto }));
                _logger.LogInformation($"Broadcasting to topic: {nameof(PlantDto)} with payload: {JsonConvert.SerializeObject(new WrapperForDto() { dto = plantDto })}");

                // Trigger watering if below threshold and auto-watering is enabled
                if (moistureData.Moisture > plant.MoistureThreshold && plant.IsAutoWateringEnabled)
                {
                    _logger.LogInformation($"Moisture below threshold. Triggering watering for Plant {plant.Id}.");
                    await wateringService.TriggerWateringAsync(plant.Id);
                    await wateringLogService.CreateAsync(new CreateWateringLogDto
                    {
                        TriggeredByUserId = null,
                        Method = WateringMethod.Auto,
                        PlantId = plant.Id
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing incoming moisture data message.");
            }
        }
    }

    

    public class WrapperForDto
    {
        public string eventType { get; set; } = nameof(PlantDto);
        public PlantDto dto { get; set; }
    }
}