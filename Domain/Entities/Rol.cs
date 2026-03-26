using Domain.Exceptions;

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
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name is required.");
        else if (name.Length > 50)
            errors.Add("Name must not exceed 50 characters.");

        if (description?.Length > 150)
            errors.Add("Description must not exceed 150 characters.");

        if (errors.Any())
            throw new ValidationException(errors);

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
