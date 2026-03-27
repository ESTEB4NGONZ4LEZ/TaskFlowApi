using Domain.Entities;
using Domain.Ports.Repositories;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRolRepository : IUserRolRepository
{
    private readonly MainContext _context;

    public UserRolRepository(MainContext context)
    {
        _context = context;
    }

    public async Task AssignAsync(UserRol userRol)
    {
        var entity = new UserRolEntity
        {
            UserId = userRol.UserId,
            RolId = userRol.RolId,
            AssignedAt = userRol.AssignedAt
        };
        await _context.UserRols.AddAsync(entity);
    }

    public async Task<bool> IsAssignedAsync(int userId, int rolId) =>
        await _context.UserRols.AnyAsync(ur => ur.UserId == userId && ur.RolId == rolId);
}
