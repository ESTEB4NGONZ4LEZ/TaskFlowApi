using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserTaskConfiguration : IEntityTypeConfiguration<UserTask>
{
    public void Configure(EntityTypeBuilder<UserTask> builder)
    {
        builder.HasKey(ut => new { ut.TaskId, ut.UserId });

        builder.Property(ut => ut.AssignedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(ut => ut.Task)
            .WithMany(t => t.UserTasks)
            .HasForeignKey(ut => ut.TaskId);

        builder.HasOne(ut => ut.User)
            .WithMany(u => u.UserTasks)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
