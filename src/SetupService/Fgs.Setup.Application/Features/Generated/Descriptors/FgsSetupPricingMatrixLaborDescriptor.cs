using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupPricingMatrixLaborDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupPricingMatrixLabor,
        EntityName: "FgsSetupPricingMatrixLabor",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupPricingMatrixLabor),
        SummaryDtoType: typeof(FgsSetupPricingMatrixLaborSummaryDto),
        DetailDtoType: typeof(FgsSetupPricingMatrixLaborDetailDto),
        CreateDtoType: typeof(FgsSetupPricingMatrixLaborCreateDto),
        UpdateDtoType: typeof(FgsSetupPricingMatrixLaborUpdateDto),
        PatchDtoType: typeof(FgsSetupPricingMatrixLaborPatchDto),
        TableName: "FgsSetupPricingMatrixLabor",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "pricingmatrixlabors",
        SwaggerTag: "Setup - Pricing",
        TableComment: "FgsSetupPricingMatrixLabor",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Id"),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "TenantId"),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "CompanyId"),
            new CatalogEntityColumnDescriptor(
                "PricingMatrixId", "PricingMatrixId", typeof(long), true, null, false, false, true, "PricingMatrixId"),
            new CatalogEntityColumnDescriptor(
                "LaborRateTypeId", "LaborRateTypeId", typeof(int), true, null, false, false, true, "LaborRateTypeId"),
            new CatalogEntityColumnDescriptor(
                "TechSkillLevelId", "TechSkillLevelId", typeof(long?), false, null, false, false, true, "TechSkillLevelId"),
            new CatalogEntityColumnDescriptor(
                "BaseRate", "BaseRate", typeof(decimal), true, null, false, false, true, "BaseRate"),
            new CatalogEntityColumnDescriptor(
                "OvertimeMultiplier", "OvertimeMultiplier", typeof(decimal?), false, null, false, false, true, "OvertimeMultiplier"),
            new CatalogEntityColumnDescriptor(
                "DoubleTimeMultiplier", "DoubleTimeMultiplier", typeof(decimal?), false, null, false, false, true, "DoubleTimeMultiplier"),
            new CatalogEntityColumnDescriptor(
                "DiscountPercent", "DiscountPercent", typeof(decimal?), false, null, false, false, true, "DiscountPercent"),
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
        SortableColumns: ["Id", "PricingMatrixId", "LaborRateTypeId", "TechSkillLevelId", "BaseRate", "OvertimeMultiplier", "DoubleTimeMultiplier", "DiscountPercent", "IsActive"]);
}
