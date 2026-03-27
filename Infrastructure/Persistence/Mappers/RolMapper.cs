using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Mappers;

public static class RolMapper
{
    public static RolEntity ToEntity(Domain.Entities.Rol model) => new()
    {
        RolId = model.RolId,
        Name = model.Name,
        Description = model.Description,
        CreatedAt = model.CreatedAt
    };

    public static Domain.Entities.Rol ToDomain(RolEntity entity) =>
        Domain.Entities.Rol.Reconstitute(entity.RolId, entity.Name, entity.Description, entity.CreatedAt);
}
