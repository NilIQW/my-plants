namespace Application.Models.Dtos.RestDtos.PlantDtos;

public class CreatePlantDto
{
    public string PlantName { get; set; } = null!;
    public string PlantType { get; set; } = null!;
    public float MoistureThreshold { get; set; }
    public float MoistureLevel { get; set; } 

    public bool IsAutoWateringEnabled { get; set; }
}