using Domain.Entities;

namespace Domain.Ports.Repositories;

public interface IStateTaskRepository : IGenericRepository<StateTask>
{
    Task<bool> ExistsWithNameAsync(string name, int excludeId = 0);
}
