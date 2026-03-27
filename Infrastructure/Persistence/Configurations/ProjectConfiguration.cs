using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<ProjectEntity>
{
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.HasKey(p => p.ProjectId);
        builder.Property(p => p.ProjectId).ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(300);

        builder.Property(p => p.CreatedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.IsActive)
            .HasColumnType("bit")
            .HasDefaultValue(true);

        builder.HasOne(p => p.CreatedByUser)
            .WithMany(u => u.Projects)
            .HasForeignKey(p => p.CreatedBy);
    }
}
