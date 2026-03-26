using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.HasKey(r => r.RolId);
        builder.Property(r => r.RolId).ValueGeneratedOnAdd();

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Description)
            .HasMaxLength(150);

        builder.Property(r => r.CreatedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()");
    }
}
