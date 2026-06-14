using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsLeadDisqualificationReasonDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.LeadDisqualificationReason,
        EntityName: "FgsLeadDisqualificationReason",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsLeadDisqualificationReason),
        SummaryDtoType: typeof(FgsLeadDisqualificationReasonSummaryDto),
        DetailDtoType: typeof(FgsLeadDisqualificationReasonDetailDto),
        CreateDtoType: typeof(FgsLeadDisqualificationReasonCreateDto),
        UpdateDtoType: typeof(FgsLeadDisqualificationReasonUpdateDto),
        PatchDtoType: typeof(FgsLeadDisqualificationReasonPatchDto),
        TableName: "FgsLeadDisqualificationReason",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "leaddisqualificationreasons",
        SwaggerTag: "Setup - Leads",
        TableComment: "FgsLeadDisqualificationReason",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Unique business code for the disqualification reason within a company."),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "TenantId"),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "CompanyId"),
            new CatalogEntityColumnDescriptor(
                "ReasonCode", "ReasonCode", typeof(string), false, 0, false, true, true, "ReasonCode"),
            new CatalogEntityColumnDescriptor(
                "ReasonName", "ReasonName", typeof(string), false, 100, false, true, true, "User-friendly name displayed throughout the application."),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 255, false, true, true, "Optional description explaining the reason."),
            new CatalogEntityColumnDescriptor(
                "DisplayOrder", "DisplayOrder", typeof(short), true, null, false, false, true, "Controls the order in which reasons are displayed in dropdowns and lists."),
            new CatalogEntityColumnDescriptor(
                "IsSystem", "IsSystem", typeof(bool), true, null, false, false, true, "Indicates whether the reason was seeded by the system or created by a user."),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "CreatedOn"),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "CreatedBy"),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "UpdatedOn"),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 0, true, false, false, "UpdatedBy"),
            new CatalogEntityColumnDescriptor(
                "IsActive", "IsActive", typeof(bool), true, null, false, false, true, "Indicates whether the reason is available for selection."),
        ],
        UniqueKeys:
        [
        ],
        SearchableColumns: ["ReasonCode", "ReasonName", "Description"],
        SortableColumns: ["Id", "ReasonCode", "ReasonName", "Description", "DisplayOrder", "IsSystem", "IsActive"]);
}
