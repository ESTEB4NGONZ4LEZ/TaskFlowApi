using System.Linq.Expressions;
using Domain.Entities;
using Domain.Ports.Repositories;
using Infrastructure.Persistence.Entities;
using Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RolRepository : GenericRepository<RolEntity, Rol>, IRolRepository
{
    public RolRepository(MainContext context) : base(context) { }

    protected override Rol ToDomain(RolEntity entity) => RolMapper.ToDomain(entity);
    protected override RolEntity ToEntity(Rol domain) => RolMapper.ToEntity(domain);
    protected override Expression<Func<RolEntity, bool>> ById(int id) => e => e.RolId == id;

    public async Task<bool> ExistsWithNameAsync(string name, int excludeId = 0) =>
        await _dbSet.AnyAsync(r => r.Name == name && r.RolId != excludeId);
}
