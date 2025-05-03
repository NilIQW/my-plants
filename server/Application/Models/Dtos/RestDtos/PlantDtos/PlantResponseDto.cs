using System.ComponentModel.DataAnnotations;

namespace Application.Models.Dtos.RestDtos.PlantDtos;

public class PlantResponseDto
{
    [Required]public string Id { get; set; }
    [Required]public string PlantName { get; set; }
    [Required]public string PlantType { get; set; }
    [Required]public double MoistureLevel { get; set; } 
    [Required]public float MoistureThreshold { get; set; }

    [Required]public bool IsAutoWateringEnabled { get; set; }
    public bool? IsOwner { get; set; }
}