using System.Text.Json;
using Application.Interfaces.Infrastructure.MQTT;
using HiveMQtt.Client;
using HiveMQtt.MQTT5.Types;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MQTT;

public class MqttPublisher(HiveMQClient client, ILogger<MqttPublisher> logger) : IMqttPublisher
{
    public async Task Publish(object dto, string topic)
    {
        var payload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        // Simulate hardware action
        logger.LogInformation("Simulating: Water pump ON for topic {Topic}", topic);
        logger.LogInformation("Payload: {Payload}", payload);

        await client.PublishAsync(topic, payload, QualityOfService.AtLeastOnceDelivery);
    }
}