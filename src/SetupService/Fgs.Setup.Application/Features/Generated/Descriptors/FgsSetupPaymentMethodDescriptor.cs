using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupPaymentMethodDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupPaymentMethod,
        EntityName: "FgsSetupPaymentMethod",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupPaymentMethod),
        SummaryDtoType: typeof(FgsSetupPaymentMethodSummaryDto),
        DetailDtoType: typeof(FgsSetupPaymentMethodDetailDto),
        CreateDtoType: typeof(FgsSetupPaymentMethodCreateDto),
        UpdateDtoType: typeof(FgsSetupPaymentMethodUpdateDto),
        PatchDtoType: typeof(FgsSetupPaymentMethodPatchDto),
        TableName: "FgsSetupPaymentMethod",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "paymentmethods",
        SwaggerTag: "Setup - Billing",
        TableComment: "FgsSetupPaymentMethod",
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
                "DisplayName", "DisplayName", typeof(string), false, 0, false, true, true, "DisplayName"),
            new CatalogEntityColumnDescriptor(
                "SortOrder", "SortOrder", typeof(int), true, null, false, false, true, "SortOrder"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsSetupPaymentMethod", ["TenantId", "CompanyId", "DisplayName"]),
        ],
        SearchableColumns: ["DisplayName"],
        SortableColumns: ["Id", "DisplayName", "SortOrder", "IsMobileVisible", "IsCustomerPortalVisible", "IsActive"]);
}
