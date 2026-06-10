using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsJobTypeDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.JobType,
        EntityName: "FgsJobType",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsJobType),
        SummaryDtoType: typeof(FgsJobTypeSummaryDto),
        DetailDtoType: typeof(FgsJobTypeDetailDto),
        CreateDtoType: typeof(FgsJobTypeCreateDto),
        UpdateDtoType: typeof(FgsJobTypeUpdateDto),
        PatchDtoType: typeof(FgsJobTypePatchDto),
        TableName: "FgsJobType",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "jobtypes",
        SwaggerTag: "Setup - JobTypes",
        TableComment: "FgsJobType",
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
                "JobTypeCategoryId", "JobTypeCategoryId", typeof(long), true, null, false, false, true, "JobTypeCategoryId"),
            new CatalogEntityColumnDescriptor(
                "JobTypeSubCategoryId", "JobTypeSubCategoryId", typeof(long?), false, null, false, false, true, "JobTypeSubCategoryId"),
            new CatalogEntityColumnDescriptor(
                "JobTypeCode", "JobTypeCode", typeof(string), false, 0, false, true, true, "JobTypeCode"),
            new CatalogEntityColumnDescriptor(
                "TaskName", "TaskName", typeof(string), false, 200, false, true, true, "TaskName"),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 50, false, true, true, "Description"),
            new CatalogEntityColumnDescriptor(
                "UsedFor", "UsedFor", typeof(string), false, 0, false, true, true, "UsedFor"),
            new CatalogEntityColumnDescriptor(
                "Trade", "Trade", typeof(string), false, 100, false, true, true, "Trade"),
            new CatalogEntityColumnDescriptor(
                "EstimatedDurationMinutes", "EstimatedDurationMinutes", typeof(int?), false, null, false, false, true, "EstimatedDurationMinutes"),
            new CatalogEntityColumnDescriptor(
                "BusinessUnit", "BusinessUnit", typeof(string), false, 100, false, true, true, "BusinessUnit"),
            new CatalogEntityColumnDescriptor(
                "Priority", "Priority", typeof(short), true, null, false, false, true, "Priority"),
            new CatalogEntityColumnDescriptor(
                "BackgroundColor", "BackgroundColor", typeof(string), false, 0, false, true, true, "BackgroundColor"),
            new CatalogEntityColumnDescriptor(
                "TextColor", "TextColor", typeof(string), false, 20, false, true, true, "TextColor"),
            new CatalogEntityColumnDescriptor(
                "ShowToFieldTech", "ShowToFieldTech", typeof(bool), true, null, false, false, true, "ShowToFieldTech"),
            new CatalogEntityColumnDescriptor(
                "ShowOnCustomerPortal", "ShowOnCustomerPortal", typeof(bool), true, null, false, false, true, "ShowOnCustomerPortal"),
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
        SearchableColumns: ["JobTypeCode", "TaskName", "Description", "UsedFor", "Trade", "BusinessUnit", "BackgroundColor", "TextColor"],
        SortableColumns: ["Id", "JobTypeCategoryId", "JobTypeSubCategoryId", "JobTypeCode", "TaskName", "Description", "UsedFor", "Trade", "EstimatedDurationMinutes", "BusinessUnit", "Priority", "BackgroundColor", "TextColor", "ShowToFieldTech", "ShowOnCustomerPortal", "DisplayOrder", "IsActive"]);
}
