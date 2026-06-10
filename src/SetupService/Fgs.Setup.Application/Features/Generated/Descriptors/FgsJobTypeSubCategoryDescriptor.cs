using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsJobTypeSubCategoryDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.JobTypeSubCategory,
        EntityName: "FgsJobTypeSubCategory",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsJobTypeSubCategory),
        SummaryDtoType: typeof(FgsJobTypeSubCategorySummaryDto),
        DetailDtoType: typeof(FgsJobTypeSubCategoryDetailDto),
        CreateDtoType: typeof(FgsJobTypeSubCategoryCreateDto),
        UpdateDtoType: typeof(FgsJobTypeSubCategoryUpdateDto),
        PatchDtoType: typeof(FgsJobTypeSubCategoryPatchDto),
        TableName: "FgsJobTypeSubCategory",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "jobtypesubcategories",
        SwaggerTag: "Setup - JobTypes",
        TableComment: "FgsJobTypeSubCategory",
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
                "SubCategoryCode", "SubCategoryCode", typeof(string), false, 0, false, true, true, "SubCategoryCode"),
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
        SearchableColumns: ["SubCategoryCode", "Name", "Description"],
        SortableColumns: ["Id", "SubCategoryCode", "Name", "Description", "DisplayOrder", "IsActive"]);
}
