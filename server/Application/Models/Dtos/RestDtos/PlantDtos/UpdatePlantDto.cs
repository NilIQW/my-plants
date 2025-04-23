namespace Application.Models.Dtos.RestDtos.PlantDtos;

public class UpdatePlantDto
{
    public string? PlantName { get; set; } = null!;
    public string? PlantType { get; set; } = null!;
    public float? MoistureLevel { get; set; }
    public float? MoistureThreshold { get; set; }
    public bool? IsAutoWateringEnabled { get; set; }
}