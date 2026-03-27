using Domain.Exceptions;

namespace Domain.Entities;

public class StateTask
{
    public int StateTaskId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }

    private StateTask() { }

    public static StateTask Create(string name, string? description)
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

        return new StateTask
        {
            Name = name,
            Description = description
        };
    }

    public void Update(string name, string? description)
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

        Name = name;
        Description = description;
    }

    public static StateTask Reconstitute(int stateTaskId, string name, string? description)
    {
        return new StateTask
        {
            StateTaskId = stateTaskId,
            Name = name,
            Description = description
        };
    }
}
