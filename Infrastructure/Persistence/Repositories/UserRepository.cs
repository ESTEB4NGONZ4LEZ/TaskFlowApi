using System.Linq.Expressions;
using Domain.Entities;
using Domain.Ports.Repositories;
using Infrastructure.Persistence.Entities;
using Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<UserEntity, User>, IUserRepository
{
    public UserRepository(MainContext context) : base(context) { }

    protected override User ToDomain(UserEntity entity) => UserMapper.ToDomain(entity);
    protected override UserEntity ToEntity(User domain) => UserMapper.ToEntity(domain);
    protected override Expression<Func<UserEntity, bool>> ById(int id) => e => e.UserId == id;

    public async Task<bool> ExistsWithUserNameAsync(string userName, int excludeId = 0) =>
        await _dbSet.AnyAsync(u => u.UserName == userName && u.UserId != excludeId);

    public async Task<bool> ExistsWithEmailAsync(string email, int excludeId = 0) =>
        await _dbSet.AnyAsync(u => u.Email == email && u.UserId != excludeId);
}
