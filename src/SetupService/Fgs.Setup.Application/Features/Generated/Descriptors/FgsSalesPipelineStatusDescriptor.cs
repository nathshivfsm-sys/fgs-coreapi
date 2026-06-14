using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSalesPipelineStatusDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SalesPipelineStatus,
        EntityName: "FgsSalesPipelineStatus",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSalesPipelineStatus),
        SummaryDtoType: typeof(FgsSalesPipelineStatusSummaryDto),
        DetailDtoType: typeof(FgsSalesPipelineStatusDetailDto),
        CreateDtoType: typeof(FgsSalesPipelineStatusCreateDto),
        UpdateDtoType: typeof(FgsSalesPipelineStatusUpdateDto),
        PatchDtoType: typeof(FgsSalesPipelineStatusPatchDto),
        TableName: "FgsSalesPipelineStatus",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "salespipelinestatuses",
        SwaggerTag: "Setup - Sales",
        TableComment: "FgsSalesPipelineStatus",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Unique identifier for the sales pipeline status."),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "Tenant identifier that owns the record."),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "Company identifier that owns the record."),
            new CatalogEntityColumnDescriptor(
                "StatusCode", "StatusCode", typeof(string), false, 0, false, true, true, "Immutable business code for the sales pipeline status."),
            new CatalogEntityColumnDescriptor(
                "StatusName", "StatusName", typeof(string), false, 100, false, true, true, "User-friendly name displayed throughout the application."),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 255, false, true, true, "Optional description explaining the purpose of the status."),
            new CatalogEntityColumnDescriptor(
                "AppliesToLead", "AppliesToLead", typeof(bool), true, null, false, false, true, "Indicates whether the status can be used by Leads."),
            new CatalogEntityColumnDescriptor(
                "AppliesToOpportunity", "AppliesToOpportunity", typeof(bool), true, null, false, false, true, "Indicates whether the status can be used by Opportunities."),
            new CatalogEntityColumnDescriptor(
                "IsTerminal", "IsTerminal", typeof(bool), true, null, false, false, true, "Indicates whether the status represents a terminal end state such as Won, Lost, or Disqualified."),
            new CatalogEntityColumnDescriptor(
                "AllowManualSelection", "AllowManualSelection", typeof(bool), true, null, false, false, true, "Indicates whether users may manually select this status."),
            new CatalogEntityColumnDescriptor(
                "DisplayOrder", "DisplayOrder", typeof(short), true, null, false, false, true, "Controls the order in which statuses are displayed."),
            new CatalogEntityColumnDescriptor(
                "IsSystem", "IsSystem", typeof(bool), true, null, false, false, true, "Indicates whether the status was seeded by the system. System records should have immutable business codes."),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "Date and time the record was created."),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "User who created the record."),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "Date and time the record was last updated."),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 0, true, false, false, "User who last updated the record."),
            new CatalogEntityColumnDescriptor(
                "IsActive", "IsActive", typeof(bool), true, null, false, false, true, "Indicates whether the status is available for use."),
        ],
        UniqueKeys:
        [
        ],
        SearchableColumns: ["StatusCode", "StatusName", "Description"],
        SortableColumns: ["Id", "StatusCode", "StatusName", "Description", "AppliesToLead", "AppliesToOpportunity", "IsTerminal", "AllowManualSelection", "DisplayOrder", "IsSystem", "IsActive"]);
}
