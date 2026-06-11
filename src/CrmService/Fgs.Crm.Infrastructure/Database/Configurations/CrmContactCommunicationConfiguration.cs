using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class CrmContactCommunicationConfiguration : IEntityTypeConfiguration<CrmContactCommunication>
{
    public void Configure(EntityTypeBuilder<CrmContactCommunication> entity)
    {
        entity.ToTable(
            "CrmContactCommunication",
            t => t.HasCheckConstraint(
                "CK_CrmContactCommunication_CommunicationTypeId",
                "\"CommunicationTypeId\" BETWEEN 1 AND 7"));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Label).HasMaxLength(100);
        entity.Property(e => e.Value).HasMaxLength(1000).IsRequired();
        entity.Property(e => e.Extension).HasMaxLength(20);
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsPrimary).HasDefaultValue(false);
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasOne<CrmContact>()
            .WithMany()
            .HasForeignKey(e => e.ContactId)
            .HasConstraintName("FK_CrmContactCommunication_Contact")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ContactId }).HasDatabaseName("IX_CrmContactCommunication_ContactId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Value }).HasDatabaseName("IX_CrmContactCommunication_Value");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CommunicationTypeId }).HasDatabaseName("IX_CrmContactCommunication_CommunicationTypeId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive }).HasDatabaseName("IX_CrmContactCommunication_IsActive");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ContactId, e.CommunicationTypeId })
            .IsUnique()
            .HasFilter("\"IsPrimary\" = true")
            .HasDatabaseName("UQ_CrmContactCommunication_Primary");
    }
}
