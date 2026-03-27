using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Mappers;

public static class UserMapper
{
    public static UserEntity ToEntity(Domain.Entities.User model) => new()
    {
        UserId = model.UserId,
        UserName = model.UserName,
        Password = model.Password,
        Email = model.Email,
        IsActive = model.IsActive,
        CreatedAt = model.CreatedAt,
        UpdatedAt = model.UpdatedAt
    };

    public static Domain.Entities.User ToDomain(UserEntity entity) =>
        Domain.Entities.User.Reconstitute(
            entity.UserId,
            entity.UserName,
            entity.Password,
            entity.Email,
            entity.IsActive,
            entity.CreatedAt,
            entity.UpdatedAt);
}
