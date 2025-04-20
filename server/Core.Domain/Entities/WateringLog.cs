using System;

namespace Core.Domain.Entities;

public class WateringLog
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PlantId { get; set; }
    public Plant Plant { get; set; } = null!;

    public string? TriggeredByUserId { get; set; } // null = automatic via ESP32
    public User? TriggeredByUser { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Method { get; set; } = "Auto"; // "Auto" or "Manual"
    public float MoistureBefore { get; set; }
    public float MoistureAfter { get; set; }
}