using System;

namespace Core.Domain.Entities;

public class WateringLog
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string PlantId { get; set; }
    public Plant Plant { get; set; } = null!;

    public string? TriggeredByUserId { get; set; } // null = automatic via ESP32
    public User? TriggeredByUser { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Method { get; set; } //should be enum auto or manual
    public float MoistureBefore { get; set; }
    public float MoistureAfter { get; set; }
}