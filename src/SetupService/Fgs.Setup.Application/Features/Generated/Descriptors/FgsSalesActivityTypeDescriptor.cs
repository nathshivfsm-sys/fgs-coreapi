using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSalesActivityTypeDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SalesActivityType,
        EntityName: "FgsSalesActivityType",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSalesActivityType),
        SummaryDtoType: typeof(FgsSalesActivityTypeSummaryDto),
        DetailDtoType: typeof(FgsSalesActivityTypeDetailDto),
        CreateDtoType: typeof(FgsSalesActivityTypeCreateDto),
        UpdateDtoType: typeof(FgsSalesActivityTypeUpdateDto),
        PatchDtoType: typeof(FgsSalesActivityTypePatchDto),
        TableName: "FgsSalesActivityType",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "salesactivitytypes",
        SwaggerTag: "Setup - Sales",
        TableComment: "FgsSalesActivityType",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Unique identifier for the sales activity type."),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "Tenant identifier that owns the record."),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "Company identifier that owns the record."),
            new CatalogEntityColumnDescriptor(
                "ActivityTypeCode", "ActivityTypeCode", typeof(string), false, 0, false, true, true, "Immutable business code for the sales activity type."),
            new CatalogEntityColumnDescriptor(
                "ActivityTypeName", "ActivityTypeName", typeof(string), false, 100, false, true, true, "User-friendly name displayed throughout the application."),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 255, false, true, true, "Optional description explaining the sales activity type."),
            new CatalogEntityColumnDescriptor(
                "AppliesToLead", "AppliesToLead", typeof(bool), true, null, false, false, true, "Indicates whether the activity type can be used by Leads."),
            new CatalogEntityColumnDescriptor(
                "AppliesToOpportunity", "AppliesToOpportunity", typeof(bool), true, null, false, false, true, "Indicates whether the activity type can be used by Opportunities."),
            new CatalogEntityColumnDescriptor(
                "AllowManualSelection", "AllowManualSelection", typeof(bool), true, null, false, false, true, "Indicates whether users may manually select this activity type."),
            new CatalogEntityColumnDescriptor(
                "DisplayOrder", "DisplayOrder", typeof(short), true, null, false, false, true, "Controls the order in which activity types are displayed."),
            new CatalogEntityColumnDescriptor(
                "IsSystem", "IsSystem", typeof(bool), true, null, false, false, true, "Indicates whether the activity type was seeded by the system. System records should have immutable business codes."),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "Date and time the record was created."),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "User who created the record."),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "Date and time the record was last updated."),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 0, true, false, false, "User who last updated the record."),
            new CatalogEntityColumnDescriptor(
                "IsActive", "IsActive", typeof(bool), true, null, false, false, true, "Indicates whether the activity type is available for use."),
        ],
        UniqueKeys:
        [
        ],
        SearchableColumns: ["ActivityTypeCode", "ActivityTypeName", "Description"],
        SortableColumns: ["Id", "ActivityTypeCode", "ActivityTypeName", "Description", "AppliesToLead", "AppliesToOpportunity", "AllowManualSelection", "DisplayOrder", "IsSystem", "IsActive"]);
}
