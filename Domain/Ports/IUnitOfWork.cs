namespace Domain.Ports;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
