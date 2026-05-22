using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloCategoryConfiguration : IEntityTypeConfiguration<GloCategory>
{
    public void Configure(EntityTypeBuilder<GloCategory> entity)
    {
        entity.ToTable("GloCategory");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityAlwaysColumn();
        entity.HasIndex(e => new { e.BusinessTypeId, e.Code })
            .IsUnique()
            .HasDatabaseName("UQ_GloCategory_BusinessTypeId_Code");
        entity.Property(e => e.Code).HasMaxLength(50);
        entity.Property(e => e.Name).HasMaxLength(100);
        entity.Property(e => e.Description).HasMaxLength(255);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasOne<GloBusinessType>()
            .WithMany()
            .HasForeignKey(e => e.BusinessTypeId)
            .HasConstraintName("FK_GloCategory_GloBusinessType_BusinessTypeId")
            .OnDelete(DeleteBehavior.Restrict);
        entity.ToTable(t => t.HasCheckConstraint("CK_GloCategory_Code_Upper", "\"Code\" = upper(\"Code\")"));
    }
}
