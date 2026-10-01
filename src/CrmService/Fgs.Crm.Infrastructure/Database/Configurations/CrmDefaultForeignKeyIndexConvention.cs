using Fgs.Crm.Domain.Entities;
using Fgs.Kernel.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

/// <summary>
/// Skips the tenant/company foreign-key index EF would add for company default tables.
/// Those tables define only the primary key and the tenant-company cache foreign key.
/// </summary>
internal sealed class CrmDefaultForeignKeyIndexConvention : ForeignKeyIndexConvention
{
    public CrmDefaultForeignKeyIndexConvention(ProviderConventionSetBuilderDependencies dependencies)
        : base(dependencies)
    {
    }

    protected override IConventionIndex? CreateIndex(
        IReadOnlyList<IConventionProperty> properties,
        bool unique,
        IConventionEntityTypeBuilder entityTypeBuilder)
    {
        var clrType = entityTypeBuilder.Metadata.ClrType;
        if ((clrType == typeof(CrmDefaultCustomer) || clrType == typeof(CrmDefaultServiceLocation))
            && properties.Count == 2
            && properties[0].Name == nameof(ITenantCompanyScoped.TenantId)
            && properties[1].Name == nameof(ITenantCompanyScoped.CompanyId))
        {
            return null;
        }

        return base.CreateIndex(properties, unique, entityTypeBuilder);
    }
}
