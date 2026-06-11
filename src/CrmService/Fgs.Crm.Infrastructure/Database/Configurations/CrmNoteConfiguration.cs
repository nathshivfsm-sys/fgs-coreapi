using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class CrmNoteConfiguration : IEntityTypeConfiguration<CrmNote>
{
    public void Configure(EntityTypeBuilder<CrmNote> entity)
    {
        entity.ToTable(
            "CrmNote",
            t =>
            {
                t.HasCheckConstraint("CK_CrmNote_EntityTypeId", "\"EntityTypeId\" BETWEEN 1 AND 5");
                t.HasCheckConstraint("CK_CrmNote_NoteTypeId", "\"NoteTypeId\" BETWEEN 1 AND 5");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Title).HasMaxLength(200);
        entity.Property(e => e.NoteText).HasColumnType("text").IsRequired();
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsPinned).HasDefaultValue(false);
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EntityTypeId, e.EntityId }).HasDatabaseName("IX_CrmNote_Entity");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.NoteTypeId }).HasDatabaseName("IX_CrmNote_NoteTypeId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsPinned }).HasDatabaseName("IX_CrmNote_IsPinned");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive }).HasDatabaseName("IX_CrmNote_IsActive");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EntityTypeId, e.EntityId, e.DisplayOrder })
            .HasDatabaseName("IX_CrmNote_Entity_DisplayOrder");
    }
}
