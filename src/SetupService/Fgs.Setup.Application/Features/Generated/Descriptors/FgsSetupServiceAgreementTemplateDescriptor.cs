using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupServiceAgreementTemplateDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupServiceAgreementTemplate,
        EntityName: "FgsSetupServiceAgreementTemplate",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupServiceAgreementTemplate),
        SummaryDtoType: typeof(FgsSetupServiceAgreementTemplateSummaryDto),
        DetailDtoType: typeof(FgsSetupServiceAgreementTemplateDetailDto),
        CreateDtoType: typeof(FgsSetupServiceAgreementTemplateCreateDto),
        UpdateDtoType: typeof(FgsSetupServiceAgreementTemplateUpdateDto),
        PatchDtoType: typeof(FgsSetupServiceAgreementTemplatePatchDto),
        TableName: "FgsSetupServiceAgreementTemplate",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "serviceagreementtemplates",
        SwaggerTag: "Setup - ServiceAgreements",
        TableComment: "FgsSetupServiceAgreementTemplate",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Discount given to service agreement customers on additional repairs."),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "TenantId"),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "CompanyId"),
            new CatalogEntityColumnDescriptor(
                "TemplateCode", "TemplateCode", typeof(string), false, 0, false, true, true, "TemplateCode"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 200, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 0, false, true, true, "Description"),
            new CatalogEntityColumnDescriptor(
                "BillingFrequencyMonths", "BillingFrequencyMonths", typeof(short), true, null, false, false, true, "BillingFrequencyMonths"),
            new CatalogEntityColumnDescriptor(
                "MaintenanceFrequencyMonths", "MaintenanceFrequencyMonths", typeof(short), true, null, false, false, true, "MaintenanceFrequencyMonths"),
            new CatalogEntityColumnDescriptor(
                "RepairDiscountPercent", "RepairDiscountPercent", typeof(decimal), true, null, false, false, true, "RepairDiscountPercent"),
            new CatalogEntityColumnDescriptor(
                "IsAutoRenew", "IsAutoRenew", typeof(bool), true, null, false, false, true, "IsAutoRenew"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsSetupServiceAgreementTemplate", ["TenantId", "CompanyId", "TemplateCode"]),
        ],
        SearchableColumns: ["TemplateCode", "Name", "Description"],
        SortableColumns: ["Id", "TemplateCode", "Name", "Description", "BillingFrequencyMonths", "MaintenanceFrequencyMonths", "RepairDiscountPercent", "IsAutoRenew", "DisplayOrder", "IsActive"]);
}
