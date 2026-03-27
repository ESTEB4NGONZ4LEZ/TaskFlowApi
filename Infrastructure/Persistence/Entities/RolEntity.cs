namespace Infrastructure.Persistence.Entities;

public class RolEntity
{
    public int RolId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<UserRolEntity> UserRols { get; set; }
}
