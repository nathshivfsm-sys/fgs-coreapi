using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupServiceAssetModelReferenceDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupServiceAssetModelReference,
        EntityName: "FgsSetupServiceAssetModelReference",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupServiceAssetModelReference),
        SummaryDtoType: typeof(FgsSetupServiceAssetModelReferenceSummaryDto),
        DetailDtoType: typeof(FgsSetupServiceAssetModelReferenceDetailDto),
        CreateDtoType: typeof(FgsSetupServiceAssetModelReferenceCreateDto),
        UpdateDtoType: typeof(FgsSetupServiceAssetModelReferenceUpdateDto),
        PatchDtoType: typeof(FgsSetupServiceAssetModelReferencePatchDto),
        TableName: "FgsSetupServiceAssetModelReference",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "serviceassetmodelreferences",
        SwaggerTag: "Setup - ServiceAssets",
        TableComment: "FgsSetupServiceAssetModelReference",
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
                "FgsSetupServiceAssetTypeId", "FgsSetupServiceAssetTypeId", typeof(long), true, null, false, false, true, "FgsSetupServiceAssetTypeId"),
            new CatalogEntityColumnDescriptor(
                "FgsSetupServiceAssetManufacturerId", "FgsSetupServiceAssetManufacturerId", typeof(long), true, null, false, false, true, "FgsSetupServiceAssetManufacturerId"),
            new CatalogEntityColumnDescriptor(
                "ModelNumber", "ModelNumber", typeof(string), false, 0, false, true, true, "ModelNumber"),
            new CatalogEntityColumnDescriptor(
                "ModelDescription", "ModelDescription", typeof(string), false, 0, false, true, true, "ModelDescription"),
            new CatalogEntityColumnDescriptor(
                "SerialNumberPattern", "SerialNumberPattern", typeof(string), false, 0, false, true, true, "SerialNumberPattern"),
            new CatalogEntityColumnDescriptor(
                "Notes", "Notes", typeof(string), false, 0, false, true, true, "Notes"),
            new CatalogEntityColumnDescriptor(
                "UrlsJson", "UrlsJson", typeof(string), false, 0, false, true, true, "UrlsJson"),
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
        SearchableColumns: ["ModelNumber", "ModelDescription", "SerialNumberPattern", "Notes", "UrlsJson"],
        SortableColumns: ["Id", "FgsSetupServiceAssetTypeId", "FgsSetupServiceAssetManufacturerId", "ModelNumber", "ModelDescription", "SerialNumberPattern", "Notes", "UrlsJson", "IsActive"]);
}
