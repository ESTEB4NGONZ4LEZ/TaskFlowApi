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

    public virtual async Task<TDomain?> GetByIdAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity is null ? null : ToDomain(entity);
    }

    public virtual async Task<IEnumerable<TDomain>> GetAllAsync()
    {
        var entities = await _dbSet.ToListAsync();
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
        var entity = await _dbSet.FindAsync(id);
        if (entity is null) return;

        var property = typeof(TEntity).GetProperty("IsActive");
        property?.SetValue(entity, false);
    }
}
