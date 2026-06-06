using Fgs.File.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.File.Infrastructure.Database.Configurations;

internal static class FgsFileDbContextConfigurationExtensions
{
    internal static void ConfigureTenantCompanySetupColumns<T>(
        this EntityTypeBuilder<T> entity,
        string? tenantCompanyIndexName = null)
        where T : FgsFile
    {
        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyId).HasColumnOrder(2);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");

        var index = entity.HasIndex(e => new { e.TenantId, e.CompanyId });
        if (tenantCompanyIndexName is not null)
        {
            index.HasDatabaseName(tenantCompanyIndexName);
        }
    }
}
