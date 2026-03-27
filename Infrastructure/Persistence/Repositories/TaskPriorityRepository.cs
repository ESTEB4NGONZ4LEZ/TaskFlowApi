using System.Linq.Expressions;
using Domain.Entities;
using Domain.Ports.Repositories;
using Infrastructure.Persistence.Entities;
using Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TaskPriorityRepository : GenericRepository<TaskPriorityEntity, TaskPriority>, ITaskPriorityRepository
{
    public TaskPriorityRepository(MainContext context) : base(context) { }

    protected override TaskPriority ToDomain(TaskPriorityEntity entity) => TaskPriorityMapper.ToDomain(entity);
    protected override TaskPriorityEntity ToEntity(TaskPriority domain) => TaskPriorityMapper.ToEntity(domain);
    protected override Expression<Func<TaskPriorityEntity, bool>> ById(int id) => e => e.TaskPriorityId == id;

    public async Task<bool> ExistsWithNameAsync(string name, int excludeId = 0) =>
        await _dbSet.AnyAsync(tp => tp.Name == name && tp.TaskPriorityId != excludeId);

    public async Task<bool> ExistsWithLevelAsync(int level, int excludeId = 0) =>
        await _dbSet.AnyAsync(tp => tp.Level == level && tp.TaskPriorityId != excludeId);
}
