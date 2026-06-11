using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class CrmContactConfiguration : IEntityTypeConfiguration<CrmContact>
{
    public void Configure(EntityTypeBuilder<CrmContact> entity)
    {
        entity.ToTable(
            "CrmContact",
            t => t.HasCheckConstraint(
                "CK_CrmContact_Owner",
                "(\"CustomerId\" IS NOT NULL AND \"ServiceLocationId\" IS NULL) OR (\"CustomerId\" IS NULL AND \"ServiceLocationId\" IS NOT NULL)"));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.DisplayName).HasMaxLength(200).IsRequired();
        entity.Property(e => e.Title).HasMaxLength(100);
        entity.Property(e => e.DepartmentName).HasMaxLength(100);
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsDefaultContact).HasDefaultValue(false);
        entity.Property(e => e.CanReceiveEstimates).HasDefaultValue(false);
        entity.Property(e => e.CanReceiveInvoices).HasDefaultValue(false);
        entity.Property(e => e.CanReceiveAppointments).HasDefaultValue(true);
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasOne<CrmCustomer>()
            .WithMany()
            .HasForeignKey(e => e.CustomerId)
            .HasConstraintName("FK_CrmContact_Customer")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<CrmServiceLocation>()
            .WithMany()
            .HasForeignKey(e => e.ServiceLocationId)
            .HasConstraintName("FK_CrmContact_ServiceLocation")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerId }).HasDatabaseName("IX_CrmContact_CustomerId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceLocationId }).HasDatabaseName("IX_CrmContact_ServiceLocationId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DisplayName }).HasDatabaseName("IX_CrmContact_DisplayName");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive }).HasDatabaseName("IX_CrmContact_IsActive");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerId })
            .IsUnique()
            .HasFilter("\"IsDefaultContact\" = true AND \"CustomerId\" IS NOT NULL")
            .HasDatabaseName("UQ_CrmContact_DefaultCustomer");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceLocationId })
            .IsUnique()
            .HasFilter("\"IsDefaultContact\" = true AND \"ServiceLocationId\" IS NOT NULL")
            .HasDatabaseName("UQ_CrmContact_DefaultServiceLocation");
    }
}
