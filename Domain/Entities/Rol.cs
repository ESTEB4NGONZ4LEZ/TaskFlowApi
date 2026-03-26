namespace Domain.Entities;

public class Rol
{
    public int RolId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Rol() { }

    public static Rol Create(string name, string description)
    {
        return new Rol
        {
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Rol Reconstitute(int rolId, string name, string description, DateTime createdAt)
    {
        return new Rol
        {
            RolId = rolId,
            Name = name,
            Description = description,
            CreatedAt = createdAt
        };
    }
}
