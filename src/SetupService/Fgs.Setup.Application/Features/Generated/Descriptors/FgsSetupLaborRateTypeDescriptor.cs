using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupLaborRateTypeDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupLaborRateType,
        EntityName: "FgsSetupLaborRateType",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupLaborRateType),
        SummaryDtoType: typeof(FgsSetupLaborRateTypeSummaryDto),
        DetailDtoType: typeof(FgsSetupLaborRateTypeDetailDto),
        CreateDtoType: typeof(FgsSetupLaborRateTypeCreateDto),
        UpdateDtoType: typeof(FgsSetupLaborRateTypeUpdateDto),
        PatchDtoType: typeof(FgsSetupLaborRateTypePatchDto),
        TableName: "FgsSetupLaborRateType",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "laborratetypes",
        SwaggerTag: "Setup - Billing",
        TableComment: "FgsSetupLaborRateType",
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
                "Name", "Name", typeof(string), false, 0, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 0, false, true, true, "Description"),
            new CatalogEntityColumnDescriptor(
                "SortOrder", "SortOrder", typeof(int), true, null, false, false, true, "SortOrder"),
            new CatalogEntityColumnDescriptor(
                "IsSystem", "IsSystem", typeof(bool), true, null, false, false, true, "IsSystem"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsSetupLaborRateType", ["TenantId", "CompanyId", "Name"]),
        ],
        SearchableColumns: ["Name", "Description"],
        SortableColumns: ["Id", "Name", "Description", "SortOrder", "IsSystem", "IsActive"]);
}
