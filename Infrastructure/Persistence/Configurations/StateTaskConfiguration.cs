using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class StateTaskConfiguration : IEntityTypeConfiguration<StateTaskEntity>
{
    public void Configure(EntityTypeBuilder<StateTaskEntity> builder)
    {
        builder.HasKey(s => s.StateTaskId);
        builder.Property(s => s.StateTaskId).ValueGeneratedOnAdd();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Description)
            .HasMaxLength(150);

        builder.HasIndex(s => s.Name).IsUnique();
    }
}
