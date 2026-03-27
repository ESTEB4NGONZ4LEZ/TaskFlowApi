using System.Linq.Expressions;
using Domain.Entities;
using Domain.Ports.Repositories;
using Infrastructure.Persistence.Entities;
using Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class StateTaskRepository : GenericRepository<StateTaskEntity, StateTask>, IStateTaskRepository
{
    public StateTaskRepository(MainContext context) : base(context) { }

    protected override StateTask ToDomain(StateTaskEntity entity) => StateTaskMapper.ToDomain(entity);
    protected override StateTaskEntity ToEntity(StateTask domain) => StateTaskMapper.ToEntity(domain);
    protected override Expression<Func<StateTaskEntity, bool>> ById(int id) => e => e.StateTaskId == id;

    public async Task<bool> ExistsWithNameAsync(string name, int excludeId = 0) =>
        await _dbSet.AnyAsync(s => s.Name == name && s.StateTaskId != excludeId);
}
