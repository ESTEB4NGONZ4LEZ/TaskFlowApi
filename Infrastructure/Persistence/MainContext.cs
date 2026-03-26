using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class MainContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<RecordStateTask> Tasks { get; set; }
    public DbSet<Rol> Rols { get; set; }
    public DbSet<StateTask> StateTasks { get; set; }
    public DbSet<Entities.Task> ToDoTasks { get; set; }
    public DbSet<TaskComment> TaskComments { get; set; }
    public DbSet<TaskPriority> TaskPriority { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRol> UserRols { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }

    public MainContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
