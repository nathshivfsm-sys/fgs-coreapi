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
            t =>
            {
                t.HasComment(
                    "Contact associated with either a customer or a service location. A contact belongs to one owner: either a customer or a service location.");
                t.HasCheckConstraint(
                    "CK_CrmContact_Owner",
                    "(\"CustomerId\" IS NOT NULL AND \"ServiceLocationId\" IS NULL) OR (\"CustomerId\" IS NULL AND \"ServiceLocationId\" IS NOT NULL)");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Primary key for the contact.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier within the tenant.");

        entity.Property(e => e.CustomerId)
            .HasComment("Customer associated with the contact. NULL when the contact belongs to a service location.");
        entity.Property(e => e.ServiceLocationId)
            .HasComment("Service location associated with the contact. NULL when the contact belongs to a customer.");

        entity.Property(e => e.DisplayName)
            .HasMaxLength(200)
            .IsRequired()
            .HasComment("Contact name displayed to users and customers.");

        entity.Property(e => e.Title)
            .HasMaxLength(100)
            .HasComment("Job title or position of the contact.");

        entity.Property(e => e.DepartmentName)
            .HasMaxLength(100)
            .HasComment("Department or organizational area of the contact.");

        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Order in which the contact is displayed relative to other contacts for the same owner.");

        entity.Property(e => e.IsDefaultContact)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the contact is the default contact for the associated customer or service location.");

        entity.Property(e => e.CanReceiveEstimates)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the contact is permitted to receive estimates.");

        entity.Property(e => e.CanReceiveInvoices)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the contact is permitted to receive invoices.");

        entity.Property(e => e.CanReceiveAppointments)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the contact is permitted to receive appointment notifications.");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the contact is active.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Timestamp when the contact was created.");

        entity.Property(e => e.CreatedBy)
            .HasComment("User or process that created the contact.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Timestamp when the contact was last updated.");

        entity.Property(e => e.UpdatedBy)
            .HasComment("User or process that last updated the contact.");

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
