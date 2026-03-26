using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.UserId);
        builder.Property(u => u.UserId).ValueGeneratedOnAdd();

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(u => u.UserName).IsUnique();

        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.IsActive)
            .HasColumnType("bit")
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(u => u.UpdatedAt)
            .HasColumnType("datetime");
    }
}
