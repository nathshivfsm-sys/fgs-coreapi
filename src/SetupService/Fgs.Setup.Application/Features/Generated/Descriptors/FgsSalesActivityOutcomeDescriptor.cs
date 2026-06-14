using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSalesActivityOutcomeDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SalesActivityOutcome,
        EntityName: "FgsSalesActivityOutcome",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSalesActivityOutcome),
        SummaryDtoType: typeof(FgsSalesActivityOutcomeSummaryDto),
        DetailDtoType: typeof(FgsSalesActivityOutcomeDetailDto),
        CreateDtoType: typeof(FgsSalesActivityOutcomeCreateDto),
        UpdateDtoType: typeof(FgsSalesActivityOutcomeUpdateDto),
        PatchDtoType: typeof(FgsSalesActivityOutcomePatchDto),
        TableName: "FgsSalesActivityOutcome",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "salesactivityoutcomes",
        SwaggerTag: "Setup - Sales",
        TableComment: "FgsSalesActivityOutcome",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Unique identifier for the sales activity outcome."),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "Tenant identifier that owns the record."),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "Company identifier that owns the record."),
            new CatalogEntityColumnDescriptor(
                "OutcomeCode", "OutcomeCode", typeof(string), false, 0, false, true, true, "Immutable business code for the sales activity outcome."),
            new CatalogEntityColumnDescriptor(
                "OutcomeName", "OutcomeName", typeof(string), false, 100, false, true, true, "User-friendly name displayed throughout the application."),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 255, false, true, true, "Optional description explaining the sales activity outcome."),
            new CatalogEntityColumnDescriptor(
                "AppliesToLead", "AppliesToLead", typeof(bool), true, null, false, false, true, "Indicates whether the outcome can be used by Leads."),
            new CatalogEntityColumnDescriptor(
                "AppliesToOpportunity", "AppliesToOpportunity", typeof(bool), true, null, false, false, true, "Indicates whether the outcome can be used by Opportunities."),
            new CatalogEntityColumnDescriptor(
                "NextSalesPipelineStatusId", "NextSalesPipelineStatusId", typeof(long?), false, null, false, false, true, "Suggested sales pipeline status that should be applied when this outcome is selected."),
            new CatalogEntityColumnDescriptor(
                "IsTerminal", "IsTerminal", typeof(bool), true, null, false, false, true, "Indicates whether selecting this outcome typically results in a terminal sales pipeline status."),
            new CatalogEntityColumnDescriptor(
                "RequireComment", "RequireComment", typeof(bool), true, null, false, false, true, "Indicates whether users must provide additional comments when selecting this outcome."),
            new CatalogEntityColumnDescriptor(
                "AllowManualSelection", "AllowManualSelection", typeof(bool), true, null, false, false, true, "Indicates whether users may manually select this outcome."),
            new CatalogEntityColumnDescriptor(
                "DisplayOrder", "DisplayOrder", typeof(short), true, null, false, false, true, "Controls the order in which outcomes are displayed."),
            new CatalogEntityColumnDescriptor(
                "IsSystem", "IsSystem", typeof(bool), true, null, false, false, true, "Indicates whether the outcome was seeded by the system. System records should have immutable business codes."),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "Date and time the record was created."),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "User who created the record."),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "Date and time the record was last updated."),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 0, true, false, false, "User who last updated the record."),
            new CatalogEntityColumnDescriptor(
                "IsActive", "IsActive", typeof(bool), true, null, false, false, true, "Indicates whether the outcome is available for use."),
        ],
        UniqueKeys:
        [
        ],
        SearchableColumns: ["OutcomeCode", "OutcomeName", "Description"],
        SortableColumns: ["Id", "OutcomeCode", "OutcomeName", "Description", "AppliesToLead", "AppliesToOpportunity", "NextSalesPipelineStatusId", "IsTerminal", "RequireComment", "AllowManualSelection", "DisplayOrder", "IsSystem", "IsActive"]);
}
