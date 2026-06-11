using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupPricingMatrixOtherDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupPricingMatrixOther,
        EntityName: "FgsSetupPricingMatrixOther",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupPricingMatrixOther),
        SummaryDtoType: typeof(FgsSetupPricingMatrixOtherSummaryDto),
        DetailDtoType: typeof(FgsSetupPricingMatrixOtherDetailDto),
        CreateDtoType: typeof(FgsSetupPricingMatrixOtherCreateDto),
        UpdateDtoType: typeof(FgsSetupPricingMatrixOtherUpdateDto),
        PatchDtoType: typeof(FgsSetupPricingMatrixOtherPatchDto),
        TableName: "FgsSetupPricingMatrixOther",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Guid,
        Variant: CatalogEntityVariant.StandardGuid,
        RoutePlural: "pricingmatrixothers",
        SwaggerTag: "Setup - Pricing",
        TableComment: "FgsSetupPricingMatrixOther",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(Guid), false, null, true, false, true, "Id"),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "TenantId"),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "CompanyId"),
            new CatalogEntityColumnDescriptor(
                "FgsSetupPricingMatrixId", "FgsSetupPricingMatrixId", typeof(Guid), true, null, false, false, true, "FgsSetupPricingMatrixId"),
            new CatalogEntityColumnDescriptor(
                "CategoryCode", "CategoryCode", typeof(string), false, 0, false, true, true, "CategoryCode"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 0, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "MarkupPercent", "MarkupPercent", typeof(decimal?), false, null, false, false, true, "MarkupPercent"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsSetupPricingMatrixOther", ["TenantId", "CompanyId", "FgsSetupPricingMatrixId", "CategoryCode"]),
        ],
        SearchableColumns: ["CategoryCode", "Name"],
        SortableColumns: ["Id", "FgsSetupPricingMatrixId", "CategoryCode", "Name", "MarkupPercent", "DiscountPercent", "IsActive"]);
}
