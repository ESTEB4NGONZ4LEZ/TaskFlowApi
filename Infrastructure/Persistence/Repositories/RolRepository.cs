using Domain.Entities;
using Domain.Ports.Repositories;
using Infrastructure.Persistence.Mappers;

namespace Infrastructure.Persistence.Repositories;

public class RolRepository : GenericRepository<Entities.Rol, Rol>, IRolRepository
{
    public RolRepository(MainContext context) : base(context) { }

    protected override Rol ToDomain(Entities.Rol entity) => RolMapper.ToDomain(entity);
    protected override Entities.Rol ToEntity(Rol domain) => RolMapper.ToEntity(domain);
}
