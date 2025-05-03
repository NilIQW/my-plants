namespace Core.Domain.Entities;

public class UserPlant
{
    public string UserId { get; set; } 
    public string PlantId { get; set; }

    public User User { get; set; } = null!;
    public Plant Plant { get; set; } = null!;

    public bool IsOwner { get; set; } = false!;
}