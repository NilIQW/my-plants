using System;
using Application.Models.Enums;

namespace Core.Domain.Entities;

public class WateringLog
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string PlantId { get; set; }
    public Plant Plant { get; set; } = null!;

    public string? TriggeredByUserId { get; set; }
    public User? TriggeredByUser { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public WateringMethod Method { get; set; }

    public float MoistureBefore { get; set; }
    public float MoistureAfter { get; set; }
}
