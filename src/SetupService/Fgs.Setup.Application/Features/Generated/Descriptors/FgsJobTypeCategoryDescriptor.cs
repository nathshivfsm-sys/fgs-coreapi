using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsJobTypeCategoryDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.JobTypeCategory,
        EntityName: "FgsJobTypeCategory",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsJobTypeCategory),
        SummaryDtoType: typeof(FgsJobTypeCategorySummaryDto),
        DetailDtoType: typeof(FgsJobTypeCategoryDetailDto),
        CreateDtoType: typeof(FgsJobTypeCategoryCreateDto),
        UpdateDtoType: typeof(FgsJobTypeCategoryUpdateDto),
        PatchDtoType: typeof(FgsJobTypeCategoryPatchDto),
        TableName: "FgsJobTypeCategory",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "jobtypecategories",
        SwaggerTag: "Setup - JobTypes",
        TableComment: "FgsJobTypeCategory",
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
                "CategoryCode", "CategoryCode", typeof(string), false, 0, false, true, true, "CategoryCode"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 150, false, true, true, "Name"),
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
        ],
        SearchableColumns: ["CategoryCode", "Name", "Description"],
        SortableColumns: ["Id", "CategoryCode", "Name", "Description", "DisplayOrder", "IsActive"]);
}
