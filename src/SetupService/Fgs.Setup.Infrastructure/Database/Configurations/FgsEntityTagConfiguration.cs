using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsEntityTagConfiguration : IEntityTypeConfiguration<FgsEntityTag>
{
    public void Configure(EntityTypeBuilder<FgsEntityTag> entity)
    {
        entity.ToTable("FgsEntityTag");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.Property(e => e.Notes).HasMaxLength(500);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TagId, e.MasterEntityTypeId, e.EntityId })
            .IsUnique()
            .HasDatabaseName("UX_FgsEntityTag_TenantId_CompanyId_TagId_MasterEntityTypeId_EntityId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.MasterEntityTypeId, e.EntityId })
            .HasDatabaseName("IX_FgsEntityTag_Entity");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TagId })
            .HasDatabaseName("IX_FgsEntityTag_TagId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.MasterEntityTypeId })
            .HasDatabaseName("IX_FgsEntityTag_MasterEntityTypeId");
        entity.HasIndex(e => e.CreatedOn)
            .IsDescending(true)
            .HasDatabaseName("IX_FgsEntityTag_CreatedOn");        entity.HasOne<FgsTag>()
            .WithMany()
            .HasForeignKey(e => e.TagId)
            .HasConstraintName("FK_FgsEntityTag_FgsTag_TagId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
