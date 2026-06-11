using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupTimeSlotDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupTimeSlot,
        EntityName: "FgsSetupTimeSlot",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupTimeSlot),
        SummaryDtoType: typeof(FgsSetupTimeSlotSummaryDto),
        DetailDtoType: typeof(FgsSetupTimeSlotDetailDto),
        CreateDtoType: typeof(FgsSetupTimeSlotCreateDto),
        UpdateDtoType: typeof(FgsSetupTimeSlotUpdateDto),
        PatchDtoType: typeof(FgsSetupTimeSlotPatchDto),
        TableName: "FgsSetupTimeSlot",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "timeslots",
        SwaggerTag: "Setup - Zone",
        TableComment: "FgsSetupTimeSlot",
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
                "FgsSetupZoneId", "FgsSetupZoneId", typeof(long?), false, null, false, false, true, "FgsSetupZoneId"),
            new CatalogEntityColumnDescriptor(
                "Code", "Code", typeof(string), false, 0, false, true, true, "Code"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 0, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "IsMobileVisible", "IsMobileVisible", typeof(bool), true, null, false, false, true, "IsMobileVisible"),
            new CatalogEntityColumnDescriptor(
                "IsCustomerPortalVisible", "IsCustomerPortalVisible", typeof(bool), true, null, false, false, true, "IsCustomerPortalVisible"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsSetupTimeSlot", ["TenantId", "CompanyId", "Code"]),
        ],
        SearchableColumns: ["Code", "Name"],
        SortableColumns: ["Id", "FgsSetupZoneId", "Code", "Name", "IsMobileVisible", "IsCustomerPortalVisible", "IsActive"]);
}
