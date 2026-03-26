using Domain.Entities;
using Domain.Ports.Repositories;
using Infrastructure.Persistence.Mappers;

namespace Infrastructure.Persistence.Repositories;

public class RolRepository : IRolRepository
{
    private readonly MainContext _context;

    public RolRepository(MainContext context)
    {
        _context = context;
    }

    public async Task<Rol> CreateAsync(Rol rol)
    {
        var entity = RolMapper.ToEntity(rol);
        await _context.Rols.AddAsync(entity);
        await _context.SaveChangesAsync();
        return RolMapper.ToDomain(entity);
    }
}
