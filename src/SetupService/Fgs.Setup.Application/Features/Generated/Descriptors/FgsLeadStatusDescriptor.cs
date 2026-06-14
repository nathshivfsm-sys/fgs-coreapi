using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsLeadStatusDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.LeadStatus,
        EntityName: "FgsLeadStatus",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsLeadStatus),
        SummaryDtoType: typeof(FgsLeadStatusSummaryDto),
        DetailDtoType: typeof(FgsLeadStatusDetailDto),
        CreateDtoType: typeof(FgsLeadStatusCreateDto),
        UpdateDtoType: typeof(FgsLeadStatusUpdateDto),
        PatchDtoType: typeof(FgsLeadStatusPatchDto),
        TableName: "FgsLeadStatus",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "leadstatuses",
        SwaggerTag: "Setup - Leads",
        TableComment: "FgsLeadStatus",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Identifier of the tenant that owns the lead status."),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "TenantId"),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "Identifier of the company that owns the lead status."),
            new CatalogEntityColumnDescriptor(
                "StatusCode", "StatusCode", typeof(string), false, 0, false, true, true, "Unique business code for the lead status within a company. Examples: NEW, CONTACTED, QUALIFIED, CONVERTED."),
            new CatalogEntityColumnDescriptor(
                "StatusName", "StatusName", typeof(string), false, 100, false, true, true, "User-friendly name displayed throughout the application."),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 255, false, true, true, "Optional description explaining the purpose of the lead status."),
            new CatalogEntityColumnDescriptor(
                "DisplayOrder", "DisplayOrder", typeof(short), true, null, false, false, true, "Determines the order in which statuses appear in dropdowns, lists, and reports."),
            new CatalogEntityColumnDescriptor(
                "IsSystem", "IsSystem", typeof(bool), true, null, false, false, true, "Indicates whether the record was seeded by the system or created by a user."),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "Date and time when the record was created."),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "User who created the record."),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "Date and time when the record was last updated."),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 0, true, false, false, "User who last updated the record."),
            new CatalogEntityColumnDescriptor(
                "IsActive", "IsActive", typeof(bool), true, null, false, false, true, "Indicates whether the status is available for selection and use."),
        ],
        UniqueKeys:
        [
        ],
        SearchableColumns: ["StatusCode", "StatusName", "Description"],
        SortableColumns: ["Id", "StatusCode", "StatusName", "Description", "DisplayOrder", "IsSystem", "IsActive"]);
}
