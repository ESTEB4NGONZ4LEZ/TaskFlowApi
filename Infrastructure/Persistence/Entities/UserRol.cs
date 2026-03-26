namespace Infrastructure.Persistence.Entities;

public class UserRol
{
    public int UserId { get; set; }
    public int RolId { get; set; }
    public DateTime AssignedAt { get; set; }

    public User User { get; set; }
    public Rol Rol { get; set; }
}