using Domain.Ports;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly MainContext _context;

    public UnitOfWork(MainContext context)
    {
        _context = context;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
