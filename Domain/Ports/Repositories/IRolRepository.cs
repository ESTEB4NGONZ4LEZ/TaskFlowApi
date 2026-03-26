using Domain.Entities;

namespace Domain.Ports.Repositories;

public interface IRolRepository : IGenericRepository<Rol>
{
    Task<bool> ExistsWithNameAsync(string name, int excludeId = 0);
}
