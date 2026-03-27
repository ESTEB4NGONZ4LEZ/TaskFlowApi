using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class MainContext : DbContext
{
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<RecordStateTaskEntity> RecordStateTasks { get; set; }
    public DbSet<RolEntity> Rols { get; set; }
    public DbSet<StateTaskEntity> StateTasks { get; set; }
    public DbSet<TaskEntity> Tasks { get; set; }
    public DbSet<TaskCommentEntity> TaskComments { get; set; }
    public DbSet<TaskPriorityEntity> TaskPriorities { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserRolEntity> UserRols { get; set; }
    public DbSet<UserTaskEntity> UserTasks { get; set; }

    public MainContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainContext).Assembly);
    }
}
