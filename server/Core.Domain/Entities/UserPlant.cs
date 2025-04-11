namespace Core.Domain.Entities;

public class UserPlant
{
    public string UserId { get; set; } = null!;
    public Guid PlantId { get; set; }

    public User User { get; set; } = null!;
    public Plant Plant { get; set; } = null!;

    public DateTime LastWatered { get; set; }
    public bool IsOwner { get; set; }
}