using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RecordStateTaskConfiguration : IEntityTypeConfiguration<RecordStateTaskEntity>
{
    public void Configure(EntityTypeBuilder<RecordStateTaskEntity> builder)
    {
        builder.HasKey(r => r.RecordStateTaskId);
        builder.Property(r => r.RecordStateTaskId).ValueGeneratedOnAdd();

        builder.Property(r => r.CreatedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(r => r.Task)
            .WithMany(t => t.RecordStateTasks)
            .HasForeignKey(r => r.TaskId);

        builder.HasOne(r => r.StateTask)
            .WithMany(s => s.RecordStateTasks)
            .HasForeignKey(r => r.StateTaskId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.ChangedByUser)
            .WithMany(u => u.RecordStateTasks)
            .HasForeignKey(r => r.ChangedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
