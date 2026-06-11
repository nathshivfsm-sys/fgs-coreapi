using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class CrmEntityTagConfiguration : IEntityTypeConfiguration<CrmEntityTag>
{
    public void Configure(EntityTypeBuilder<CrmEntityTag> entity)
    {
        entity.ToTable("CrmEntityTag");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TagId, e.EntityTypeId, e.EntityId })
            .IsUnique()
            .HasDatabaseName("UQ_CrmEntityTag");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EntityTypeId, e.EntityId })
            .HasDatabaseName("IX_CrmEntityTag_Entity");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TagId })
            .HasDatabaseName("IX_CrmEntityTag_TagId");
    }
}
