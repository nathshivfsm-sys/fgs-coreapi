using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsBusinessTypeDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.BusinessType,
        EntityName: "FgsBusinessType",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsBusinessType),
        SummaryDtoType: typeof(FgsBusinessTypeSummaryDto),
        DetailDtoType: typeof(FgsBusinessTypeDetailDto),
        CreateDtoType: typeof(FgsBusinessTypeCreateDto),
        UpdateDtoType: typeof(FgsBusinessTypeUpdateDto),
        PatchDtoType: typeof(FgsBusinessTypePatchDto),
        TableName: "FgsBusinessType",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "businesstypes",
        SwaggerTag: "Setup - Business",
        TableComment: "FgsBusinessType",
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
                "Name", "Name", typeof(string), false, 200, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 0, false, true, true, "Description"),
            new CatalogEntityColumnDescriptor(
                "DisplayOrder", "DisplayOrder", typeof(short), true, null, false, false, true, "DisplayOrder"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsBusinessType", ["TenantId", "CompanyId", "Code"]),
        ],
        SearchableColumns: ["Code", "Name", "Description"],
        SortableColumns: ["Id", "Code", "Name", "Description", "DisplayOrder", "IsActive"]);
}
