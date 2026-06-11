using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupPricingMatrixLaborTierDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupPricingMatrixLaborTier,
        EntityName: "FgsSetupPricingMatrixLaborTier",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupPricingMatrixLaborTier),
        SummaryDtoType: typeof(FgsSetupPricingMatrixLaborTierSummaryDto),
        DetailDtoType: typeof(FgsSetupPricingMatrixLaborTierDetailDto),
        CreateDtoType: typeof(FgsSetupPricingMatrixLaborTierCreateDto),
        UpdateDtoType: typeof(FgsSetupPricingMatrixLaborTierUpdateDto),
        PatchDtoType: typeof(FgsSetupPricingMatrixLaborTierPatchDto),
        TableName: "FgsSetupPricingMatrixLaborTier",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "pricingmatrixlabortiers",
        SwaggerTag: "Setup - Pricing",
        TableComment: "FgsSetupPricingMatrixLaborTier",
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
                "FgsSetupPricingMatrixLaborId", "FgsSetupPricingMatrixLaborId", typeof(Guid), true, null, false, false, true, "FgsSetupPricingMatrixLaborId"),
            new CatalogEntityColumnDescriptor(
                "SequenceOrder", "SequenceOrder", typeof(int), true, null, false, false, true, "SequenceOrder"),
            new CatalogEntityColumnDescriptor(
                "DurationMinutes", "DurationMinutes", typeof(int), true, null, false, false, true, "DurationMinutes"),
            new CatalogEntityColumnDescriptor(
                "Rate", "Rate", typeof(decimal), true, null, false, false, true, "Rate"),
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
        SortableColumns: ["Id", "FgsSetupPricingMatrixLaborId", "SequenceOrder", "DurationMinutes", "Rate", "IsActive"]);
}
