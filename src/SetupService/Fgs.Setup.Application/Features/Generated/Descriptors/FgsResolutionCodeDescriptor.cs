using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsResolutionCodeDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.ResolutionCode,
        EntityName: "FgsResolutionCode",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsResolutionCode),
        SummaryDtoType: typeof(FgsResolutionCodeSummaryDto),
        DetailDtoType: typeof(FgsResolutionCodeDetailDto),
        CreateDtoType: typeof(FgsResolutionCodeCreateDto),
        UpdateDtoType: typeof(FgsResolutionCodeUpdateDto),
        PatchDtoType: typeof(FgsResolutionCodePatchDto),
        TableName: "FgsResolutionCode",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "resolutioncodes",
        SwaggerTag: "Setup - Communication",
        TableComment: "FgsResolutionCode",
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
                "GloResolutionTypeId", "GloResolutionTypeId", typeof(int), true, null, false, false, true, "GloResolutionTypeId"),
            new CatalogEntityColumnDescriptor(
                "ResolutionCode", "ResolutionCode", typeof(string), false, 0, false, true, true, "ResolutionCode"),
            new CatalogEntityColumnDescriptor(
                "ResolutionName", "ResolutionName", typeof(string), false, 200, false, true, true, "ResolutionName"),
            new CatalogEntityColumnDescriptor(
                "IsMobileVisible", "IsMobileVisible", typeof(bool), true, null, false, false, true, "IsMobileVisible"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsResolutionCode", ["TenantId", "CompanyId", "ResolutionCode"]),
        ],
        SearchableColumns: ["ResolutionCode", "ResolutionName"],
        SortableColumns: ["Id", "GloResolutionTypeId", "ResolutionCode", "ResolutionName", "IsMobileVisible", "IsActive"]);
}
