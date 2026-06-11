using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupPricingMatrixMaterialTierDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupPricingMatrixMaterialTier,
        EntityName: "FgsSetupPricingMatrixMaterialTier",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupPricingMatrixMaterialTier),
        SummaryDtoType: typeof(FgsSetupPricingMatrixMaterialTierSummaryDto),
        DetailDtoType: typeof(FgsSetupPricingMatrixMaterialTierDetailDto),
        CreateDtoType: typeof(FgsSetupPricingMatrixMaterialTierCreateDto),
        UpdateDtoType: typeof(FgsSetupPricingMatrixMaterialTierUpdateDto),
        PatchDtoType: typeof(FgsSetupPricingMatrixMaterialTierPatchDto),
        TableName: "FgsSetupPricingMatrixMaterialTier",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "pricingmatrixmaterialtiers",
        SwaggerTag: "Setup - Pricing",
        TableComment: "FgsSetupPricingMatrixMaterialTier",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Reference to the pricing matrix that contains this tier."),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "TenantId"),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "CompanyId"),
            new CatalogEntityColumnDescriptor(
                "FgsSetupPricingMatrixId", "FgsSetupPricingMatrixId", typeof(Guid), true, null, false, false, true, "FgsSetupPricingMatrixId"),
            new CatalogEntityColumnDescriptor(
                "FromCost", "FromCost", typeof(decimal), true, null, false, false, true, "Inclusive minimum material cost for this pricing tier."),
            new CatalogEntityColumnDescriptor(
                "ToCost", "ToCost", typeof(decimal?), false, null, false, false, true, "Inclusive maximum material cost for this pricing tier. NULL indicates no upper limit."),
            new CatalogEntityColumnDescriptor(
                "AdjustmentValue", "AdjustmentValue", typeof(decimal), true, null, false, false, true, "Value used by the selected pricing adjustment type. Examples: 25 = 25% markup, 150 = $150 markup, 1.75 = multiplier."),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "CreatedOn"),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "CreatedBy"),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "UpdatedOn"),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 0, true, false, false, "UpdatedBy"),
            new CatalogEntityColumnDescriptor(
                "IsActive", "IsActive", typeof(bool), true, null, false, false, true, "IsActive"),
        ],
        UniqueKeys:
        [
        ],
        SearchableColumns: [],
        SortableColumns: ["Id", "FgsSetupPricingMatrixId", "FromCost", "ToCost", "AdjustmentValue", "IsActive"]);
}
