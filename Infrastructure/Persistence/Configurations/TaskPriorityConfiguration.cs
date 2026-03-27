using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TaskPriorityConfiguration : IEntityTypeConfiguration<TaskPriorityEntity>
{
    public void Configure(EntityTypeBuilder<TaskPriorityEntity> builder)
    {
        builder.HasKey(tp => tp.TaskPriorityId);
        builder.Property(tp => tp.TaskPriorityId).ValueGeneratedOnAdd();

        builder.Property(tp => tp.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(tp => tp.Description)
            .HasMaxLength(150);

        builder.Property(tp => tp.Level);
    }
}
