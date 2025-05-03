namespace Core.Domain.Entities;

public class Plant
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string PlantName { get; set; } = null!;
    public string PlantType { get; set; } = null!;
    public float MoistureLevel { get; set; } 
    public float MoistureThreshold { get; set; } 
    public bool IsAutoWateringEnabled { get; set; }

    public ICollection<UserPlant> UserPlants { get; set; } = new List<UserPlant>();
    public ICollection<WateringLog> WateringLogs { get; set; } = new List<WateringLog>();

}