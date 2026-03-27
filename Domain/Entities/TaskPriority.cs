using Domain.Exceptions;

namespace Domain.Entities;

public class TaskPriority
{
    public int TaskPriorityId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public int Level { get; private set; }

    private TaskPriority() { }

    public static TaskPriority Create(string name, string? description, int level)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name is required.");
        else if (name.Length > 50)
            errors.Add("Name must not exceed 50 characters.");

        if (description is not null && description.Length > 150)
            errors.Add("Description must not exceed 150 characters.");

        if (level < 1)
            errors.Add("Level must be greater than or equal to 1.");

        if (errors.Any())
            throw new ValidationException(errors);

        return new TaskPriority { Name = name, Description = description, Level = level };
    }

    public void Update(string name, string? description, int level)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name is required.");
        else if (name.Length > 50)
            errors.Add("Name must not exceed 50 characters.");

        if (description is not null && description.Length > 150)
            errors.Add("Description must not exceed 150 characters.");

        if (level < 1)
            errors.Add("Level must be greater than or equal to 1.");

        if (errors.Any())
            throw new ValidationException(errors);

        Name = name;
        Description = description;
        Level = level;
    }

    public static TaskPriority Reconstitute(int taskPriorityId, string name, string? description, int level) =>
        new() { TaskPriorityId = taskPriorityId, Name = name, Description = description, Level = level };
}
