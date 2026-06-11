using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsLeadSourceDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.LeadSource,
        EntityName: "FgsLeadSource",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsLeadSource),
        SummaryDtoType: typeof(FgsLeadSourceSummaryDto),
        DetailDtoType: typeof(FgsLeadSourceDetailDto),
        CreateDtoType: typeof(FgsLeadSourceCreateDto),
        UpdateDtoType: typeof(FgsLeadSourceUpdateDto),
        PatchDtoType: typeof(FgsLeadSourcePatchDto),
        TableName: "FgsLeadSource",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "leadsources",
        SwaggerTag: "Setup - JobTypes",
        TableComment: "FgsLeadSource",
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
                "SourceCode", "SourceCode", typeof(string), false, 0, false, true, true, "SourceCode"),
            new CatalogEntityColumnDescriptor(
                "SourceName", "SourceName", typeof(string), false, 100, false, true, true, "SourceName"),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 255, false, true, true, "Description"),
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
        SearchableColumns: ["SourceCode", "SourceName", "Description"],
        SortableColumns: ["Id", "SourceCode", "SourceName", "Description", "IsActive"]);
}
