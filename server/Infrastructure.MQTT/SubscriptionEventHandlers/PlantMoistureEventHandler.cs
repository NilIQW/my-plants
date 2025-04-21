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

    // Inject logger to log any issues
    public PlantMoistureMessageHandler(ILogger<PlantMoistureMessageHandler> logger)
    {
        _logger = logger;
    }

    // Handle the incoming message
    public void Handle(object? sender, OnMessageReceivedEventArgs args)
    {
        try
        {
            var payload = args.PublishMessage.PayloadAsString;
            
            
            // Deserialize the payload to the expected class
            var moistureData = JsonConvert.DeserializeObject<PlantMoistureData>(payload);
            

            if (moistureData != null)
            {
                _logger.LogInformation($"Received moisture data: plantId = {moistureData.PlantId}, moisture = {moistureData.Moisture}");
                
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
    public string PlantId { get; set; }
    public int Moisture { get; set; }
}
