using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
{
    public void Configure(EntityTypeBuilder<TaskComment> builder)
    {
        builder.HasKey(tc => tc.TaskCommentId);
        builder.Property(tc => tc.TaskCommentId).ValueGeneratedOnAdd();

        builder.Property(tc => tc.Description)
            .HasColumnType("varchar(max)");

        builder.Property(tc => tc.CreatedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(tc => tc.Task)
            .WithMany(t => t.TaskComments)
            .HasForeignKey(tc => tc.TaskId);

        builder.HasOne(tc => tc.CreatedByUser)
            .WithMany(u => u.TaskComments)
            .HasForeignKey(tc => tc.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
