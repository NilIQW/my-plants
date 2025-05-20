using Application.Models.Enums;

namespace Application.Models.Dtos.RestDtos.WateringLogDtos;

public class CreateWateringLogDto
{
    public string? TriggeredByUserId { get; set; }
    public WateringMethod Method { get; set; }
    public string PlantId { get; set; }

}