namespace Infrastructure.Persistence.Entities;

public class UserRolEntity
{
    public int UserId { get; set; }
    public int RolId { get; set; }
    public DateTime AssignedAt { get; set; }

    public UserEntity User { get; set; }
    public RolEntity Rol { get; set; }
}
