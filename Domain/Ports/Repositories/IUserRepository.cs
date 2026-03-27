using Domain.Entities;

namespace Domain.Ports.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> ExistsWithUserNameAsync(string userName, int excludeId = 0);
    Task<bool> ExistsWithEmailAsync(string email, int excludeId = 0);
}
