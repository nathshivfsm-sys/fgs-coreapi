using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class CrmServiceLocationConfiguration : IEntityTypeConfiguration<CrmServiceLocation>
{
    public void Configure(EntityTypeBuilder<CrmServiceLocation> entity)
    {
        entity.ToTable("CrmServiceLocation");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.LocationNumber).HasMaxLength(50).IsRequired();

        entity.HasOne<CrmCustomer>()
            .WithMany()
            .HasForeignKey(e => e.CustomerId)
            .HasConstraintName("FK_CrmServiceLocation_Customer")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LocationNumber })
            .IsUnique()
            .HasDatabaseName("UQ_CrmServiceLocation_LocationNumber");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerId, e.LocationSequence })
            .IsUnique()
            .HasDatabaseName("UQ_CrmServiceLocation_Customer_LocationSequence");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerId })
            .HasDatabaseName("IX_CrmServiceLocation_CustomerId");
    }
}
