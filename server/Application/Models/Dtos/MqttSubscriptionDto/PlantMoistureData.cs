namespace Application.Models.Dtos.MqttSubscriptionDto;

public class PlantMoistureData
{
    public string PlantId { get; set; }
    public int Moisture { get; set; }
}