namespace Infrastructure.Persistence.Entities;

public class Rol
{
    public int RolId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public List<UserRol> UserRols { get; set; }
}