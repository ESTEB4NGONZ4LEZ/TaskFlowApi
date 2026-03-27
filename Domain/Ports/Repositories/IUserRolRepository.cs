using Domain.Entities;

namespace Domain.Ports.Repositories;

public interface IUserRolRepository
{
    Task AssignAsync(UserRol userRol);
    Task<bool> IsAssignedAsync(int userId, int rolId);
}
