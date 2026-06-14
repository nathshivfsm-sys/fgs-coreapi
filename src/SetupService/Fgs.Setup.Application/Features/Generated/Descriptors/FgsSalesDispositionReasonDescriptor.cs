using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSalesDispositionReasonDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SalesDispositionReason,
        EntityName: "FgsSalesDispositionReason",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSalesDispositionReason),
        SummaryDtoType: typeof(FgsSalesDispositionReasonSummaryDto),
        DetailDtoType: typeof(FgsSalesDispositionReasonDetailDto),
        CreateDtoType: typeof(FgsSalesDispositionReasonCreateDto),
        UpdateDtoType: typeof(FgsSalesDispositionReasonUpdateDto),
        PatchDtoType: typeof(FgsSalesDispositionReasonPatchDto),
        TableName: "FgsSalesDispositionReason",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "salesdispositionreasons",
        SwaggerTag: "Setup - Sales",
        TableComment: "FgsSalesDispositionReason",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Unique identifier for the sales disposition reason."),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "Tenant identifier that owns the record."),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "Company identifier that owns the record."),
            new CatalogEntityColumnDescriptor(
                "DispositionReasonCode", "DispositionReasonCode", typeof(string), false, 0, false, true, true, "Immutable business code for the disposition reason."),
            new CatalogEntityColumnDescriptor(
                "DispositionReasonName", "DispositionReasonName", typeof(string), false, 100, false, true, true, "User-friendly name displayed throughout the application."),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 255, false, true, true, "Optional description explaining the disposition reason."),
            new CatalogEntityColumnDescriptor(
                "AppliesToLead", "AppliesToLead", typeof(bool), true, null, false, false, true, "Indicates whether the reason can be used when a Lead is Disqualified."),
            new CatalogEntityColumnDescriptor(
                "AppliesToOpportunity", "AppliesToOpportunity", typeof(bool), true, null, false, false, true, "Indicates whether the reason can be used when an Opportunity is Lost."),
            new CatalogEntityColumnDescriptor(
                "RequireComment", "RequireComment", typeof(bool), true, null, false, false, true, "Indicates whether users must provide additional comments when selecting this disposition reason."),
            new CatalogEntityColumnDescriptor(
                "IsTerminal", "IsTerminal", typeof(bool), true, null, false, false, true, "Indicates whether selecting this disposition reason should result in a terminal pipeline status."),
            new CatalogEntityColumnDescriptor(
                "AllowManualSelection", "AllowManualSelection", typeof(bool), true, null, false, false, true, "Indicates whether users may manually select this disposition reason."),
            new CatalogEntityColumnDescriptor(
                "DisplayOrder", "DisplayOrder", typeof(short), true, null, false, false, true, "Controls the order in which disposition reasons are displayed."),
            new CatalogEntityColumnDescriptor(
                "IsSystem", "IsSystem", typeof(bool), true, null, false, false, true, "Indicates whether the disposition reason was seeded by the system. System records should have immutable business codes."),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "Date and time the record was created."),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "User who created the record."),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "Date and time the record was last updated."),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 0, true, false, false, "User who last updated the record."),
            new CatalogEntityColumnDescriptor(
                "IsActive", "IsActive", typeof(bool), true, null, false, false, true, "Indicates whether the disposition reason is available for use."),
        ],
        UniqueKeys:
        [
        ],
        SearchableColumns: ["DispositionReasonCode", "DispositionReasonName", "Description"],
        SortableColumns: ["Id", "DispositionReasonCode", "DispositionReasonName", "Description", "AppliesToLead", "AppliesToOpportunity", "RequireComment", "IsTerminal", "AllowManualSelection", "DisplayOrder", "IsSystem", "IsActive"]);
}
