using Application.Models.Enums;

namespace Application.Models.Dtos.RestDtos.WateringLogDtos;

public class WateringLogDto
{
    public string Id { get; set; } = null!;
    public string PlantId { get; set; } = null!;
    public string? TriggeredByUserId { get; set; }
    public DateTime Timestamp { get; set; }
    public WateringMethod Method { get; set; }
}