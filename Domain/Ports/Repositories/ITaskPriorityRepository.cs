using Domain.Entities;

namespace Domain.Ports.Repositories;

public interface ITaskPriorityRepository : IGenericRepository<TaskPriority>
{
    Task<bool> ExistsWithNameAsync(string name, int excludeId = 0);
    Task<bool> ExistsWithLevelAsync(int level, int excludeId = 0);
}
