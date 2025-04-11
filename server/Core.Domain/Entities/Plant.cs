namespace Core.Domain.Entities;

public class Plant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PlantName { get; set; } = null!;
    public string PlantType { get; set; } = null!;
    public float MoistureLevel { get; set; } // e.g., 0.0 - 100.0
    public float MoistureThreshold { get; set; } // below this = dry
    public bool IsAutoWateringEnabled { get; set; }

    public ICollection<UserPlant> UserPlants { get; set; } = new List<UserPlant>();
    public ICollection<WateringLog> WateringLogs { get; set; } = new List<WateringLog>();

}