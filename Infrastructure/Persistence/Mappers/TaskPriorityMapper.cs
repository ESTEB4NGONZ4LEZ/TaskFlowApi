using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Mappers;

public static class TaskPriorityMapper
{
    public static TaskPriorityEntity ToEntity(Domain.Entities.TaskPriority model) => new()
    {
        TaskPriorityId = model.TaskPriorityId,
        Name = model.Name,
        Description = model.Description,
        Level = model.Level
    };

    public static Domain.Entities.TaskPriority ToDomain(TaskPriorityEntity entity) =>
        Domain.Entities.TaskPriority.Reconstitute(
            entity.TaskPriorityId,
            entity.Name,
            entity.Description,
            entity.Level);
}
