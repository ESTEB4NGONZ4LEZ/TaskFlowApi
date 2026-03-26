namespace Domain.Ports.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<Func<T>> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task InactivateAsync(int id);
}
