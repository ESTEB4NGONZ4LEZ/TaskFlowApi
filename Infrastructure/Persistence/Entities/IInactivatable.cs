namespace Infrastructure.Persistence.Entities;

public interface IInactivatable
{
    bool IsActive { get; set; }
}
