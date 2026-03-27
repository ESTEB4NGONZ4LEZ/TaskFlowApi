using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Mappers;

public static class StateTaskMapper
{
    public static StateTaskEntity ToEntity(Domain.Entities.StateTask model) => new()
    {
        StateTaskId = model.StateTaskId,
        Name = model.Name,
        Description = model.Description
    };

    public static Domain.Entities.StateTask ToDomain(StateTaskEntity entity) =>
        Domain.Entities.StateTask.Reconstitute(entity.StateTaskId, entity.Name, entity.Description);
}
