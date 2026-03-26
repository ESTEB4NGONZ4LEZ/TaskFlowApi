using Domain.Entities;

namespace Domain.Ports.Repositories;

public interface IRolRepository
{
    Task<Rol> CreateAsync(Rol rol);
}
