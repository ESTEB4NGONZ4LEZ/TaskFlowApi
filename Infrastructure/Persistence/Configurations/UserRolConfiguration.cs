using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserRolConfiguration : IEntityTypeConfiguration<UserRolEntity>
{
    public void Configure(EntityTypeBuilder<UserRolEntity> builder)
    {
        builder.HasKey(ur => new { ur.UserId, ur.RolId });

        builder.Property(ur => ur.AssignedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(ur => ur.User)
            .WithMany(u => u.UserRols)
            .HasForeignKey(ur => ur.UserId);

        builder.HasOne(ur => ur.Rol)
            .WithMany(r => r.UserRols)
            .HasForeignKey(ur => ur.RolId);
    }
}
