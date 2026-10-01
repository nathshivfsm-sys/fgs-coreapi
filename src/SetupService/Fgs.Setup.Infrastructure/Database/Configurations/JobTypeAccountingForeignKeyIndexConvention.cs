using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

/// <summary>
/// Skips the single-column foreign-key indexes EF would add for job-type accounting.
/// Those columns are already covered by the named composite indexes.
/// </summary>
internal sealed class JobTypeAccountingForeignKeyIndexConvention : ForeignKeyIndexConvention
{
    public JobTypeAccountingForeignKeyIndexConvention(ProviderConventionSetBuilderDependencies dependencies)
        : base(dependencies)
    {
    }

    protected override IConventionIndex? CreateIndex(
        IReadOnlyList<IConventionProperty> properties,
        bool unique,
        IConventionEntityTypeBuilder entityTypeBuilder)
    {
        var clrType = entityTypeBuilder.Metadata.ClrType;
        if ((clrType == typeof(FgsJobTypeAccounting) || clrType == typeof(FgsJobTypeBillingCategoryAccounting))
            && properties.Count == 1
            && properties[0].Name is nameof(FgsJobTypeAccounting.JobTypeId)
                or nameof(FgsJobTypeBillingCategoryAccounting.BillingCategoryId))
        {
            return null;
        }

        return base.CreateIndex(properties, unique, entityTypeBuilder);
    }
}
