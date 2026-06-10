using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsTagDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.Tag,
        EntityName: "FgsTag",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsTag),
        SummaryDtoType: typeof(FgsTagSummaryDto),
        DetailDtoType: typeof(FgsTagDetailDto),
        CreateDtoType: typeof(FgsTagCreateDto),
        UpdateDtoType: typeof(FgsTagUpdateDto),
        PatchDtoType: typeof(FgsTagPatchDto),
        TableName: "FgsTag",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.ScopedManualAudit,
        RoutePlural: "tags",
        SwaggerTag: "Setup - Tags",
        TableComment: "FgsTag",
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
                "TagCode", "TagCode", typeof(string), false, 0, false, true, true, "TagCode"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 100, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "NormalizedName", "NormalizedName", typeof(string), false, 100, false, true, true, "NormalizedName"),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 0, false, true, true, "Description"),
            new CatalogEntityColumnDescriptor(
                "BackgroundColor", "BackgroundColor", typeof(string), false, 20, false, true, true, "BackgroundColor"),
            new CatalogEntityColumnDescriptor(
                "TextColor", "TextColor", typeof(string), false, 20, false, true, true, "TextColor"),
            new CatalogEntityColumnDescriptor(
                "IconFileId", "IconFileId", typeof(long?), false, null, false, false, true, "IconFileId"),
            new CatalogEntityColumnDescriptor(
                "UsageCount", "UsageCount", typeof(int), true, null, false, false, true, "UsageCount"),
            new CatalogEntityColumnDescriptor(
                "IsSystemGenerated", "IsSystemGenerated", typeof(bool), true, null, false, false, true, "IsSystemGenerated"),
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
        SearchableColumns: ["TagCode", "Name", "NormalizedName", "Description", "BackgroundColor", "TextColor"],
        SortableColumns: ["Id", "TagCode", "Name", "NormalizedName", "Description", "BackgroundColor", "TextColor", "IconFileId", "UsageCount", "IsSystemGenerated", "IsActive"]);
}
