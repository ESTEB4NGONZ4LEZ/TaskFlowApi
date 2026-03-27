using System.Linq.Expressions;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public abstract class GenericRepository<TEntity, TDomain>
    where TEntity : class
    where TDomain : class
{
    protected readonly MainContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected GenericRepository(MainContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    protected abstract TDomain ToDomain(TEntity entity);
    protected abstract TEntity ToEntity(TDomain domain);
    protected abstract Expression<Func<TEntity, bool>> ById(int id);

    public virtual async Task<TDomain?> GetByIdAsync(int id)
    {
        var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(ById(id));
        return entity is null ? null : ToDomain(entity);
    }

    public virtual async Task<bool> ExistsAsync(int id) =>
        await _dbSet.AnyAsync(ById(id));

    public virtual async Task<IEnumerable<TDomain>> GetAllAsync()
    {
        var entities = await _dbSet.AsNoTracking().ToListAsync();
        return entities.Select(ToDomain);
    }

    public virtual async Task<Func<TDomain>> CreateAsync(TDomain domain)
    {
        var entity = ToEntity(domain);
        await _dbSet.AddAsync(entity);
        return () => ToDomain(entity);
    }

    public virtual Task UpdateAsync(TDomain domain)
    {
        var entity = ToEntity(domain);
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual async Task InactivateAsync(int id)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(ById(id));
        if (entity is null) return;

        if (entity is IInactivatable inactivatable)
            inactivatable.IsActive = false;
    }
}
