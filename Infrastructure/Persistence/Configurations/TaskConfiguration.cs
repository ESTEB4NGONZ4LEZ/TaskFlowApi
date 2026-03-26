using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskEntity = Infrastructure.Persistence.Entities.Task;

namespace Infrastructure.Persistence.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.HasKey(t => t.TaskId);
        builder.Property(t => t.TaskId).ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Description)
            .HasMaxLength(300);

        builder.Property(t => t.StartDate)
            .HasColumnType("datetime");

        builder.Property(t => t.DueDate)
            .HasColumnType("datetime");

        builder.Property(t => t.CompletedAt)
            .HasColumnType("datetime");

        builder.Property(t => t.CreatedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(t => t.UpdatedAt)
            .HasColumnType("datetime");

        builder.HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId);

        builder.HasOne(t => t.StateTask)
            .WithMany(s => s.Tasks)
            .HasForeignKey(t => t.StateTaskId);

        builder.HasOne(t => t.TaskPriority)
            .WithMany(tp => tp.Tasks)
            .HasForeignKey(t => t.TaskPriorityId);

        builder.HasOne(t => t.CreatedByUser)
            .WithMany(u => u.CreatedTasks)
            .HasForeignKey(t => t.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.AssignedToUser)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssignedTo)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
