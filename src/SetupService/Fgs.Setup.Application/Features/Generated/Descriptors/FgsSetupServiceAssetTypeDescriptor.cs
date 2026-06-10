using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupServiceAssetTypeDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupServiceAssetType,
        EntityName: "FgsSetupServiceAssetType",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupServiceAssetType),
        SummaryDtoType: typeof(FgsSetupServiceAssetTypeSummaryDto),
        DetailDtoType: typeof(FgsSetupServiceAssetTypeDetailDto),
        CreateDtoType: typeof(FgsSetupServiceAssetTypeCreateDto),
        UpdateDtoType: typeof(FgsSetupServiceAssetTypeUpdateDto),
        PatchDtoType: typeof(FgsSetupServiceAssetTypePatchDto),
        TableName: "FgsSetupServiceAssetType",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "serviceassettypes",
        SwaggerTag: "Setup - ServiceAssets",
        TableComment: "FgsSetupServiceAssetType",
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
                "Code", "Code", typeof(string), false, 0, false, true, true, "Code"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 0, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 0, false, true, true, "Description"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsSetupServiceAssetType", ["TenantId", "CompanyId", "Code"]),
        ],
        SearchableColumns: ["Code", "Name", "Description"],
        SortableColumns: ["Id", "Code", "Name", "Description", "IsActive"]);
}
