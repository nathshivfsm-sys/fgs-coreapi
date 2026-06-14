using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupServiceAgreementTemplatePricingComponentDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupServiceAgreementTemplatePricingComponent,
        EntityName: "FgsSetupServiceAgreementTemplatePricingComponent",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupServiceAgreementTemplatePricingComponent),
        SummaryDtoType: typeof(FgsSetupServiceAgreementTemplatePricingComponentSummaryDto),
        DetailDtoType: typeof(FgsSetupServiceAgreementTemplatePricingComponentDetailDto),
        CreateDtoType: typeof(FgsSetupServiceAgreementTemplatePricingComponentCreateDto),
        UpdateDtoType: typeof(FgsSetupServiceAgreementTemplatePricingComponentUpdateDto),
        PatchDtoType: typeof(FgsSetupServiceAgreementTemplatePricingComponentPatchDto),
        TableName: "FgsSetupServiceAgreementTemplatePricingComponent",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "serviceagreementtemplatepricingcomponents",
        SwaggerTag: "Setup - ServiceAgreements",
        TableComment: "FgsSetupServiceAgreementTemplatePricingComponent",
        SupportsSoftDelete: false,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Service agreement template that includes this pricing component snapshot."),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "TenantId"),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "CompanyId"),
            new CatalogEntityColumnDescriptor(
                "ServiceAgreementTemplateId", "ServiceAgreementTemplateId", typeof(long), true, null, false, false, true, "ServiceAgreementTemplateId"),
            new CatalogEntityColumnDescriptor(
                "PricingComponentCode", "PricingComponentCode", typeof(string), false, 0, false, true, true, "PricingComponentCode"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 200, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "Amount", "Amount", typeof(decimal), true, null, false, false, true, "Amount"),
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
        ],
        UniqueKeys:
        [
        ],
        SearchableColumns: ["PricingComponentCode", "Name"],
        SortableColumns: ["Id", "ServiceAgreementTemplateId", "PricingComponentCode", "Name", "Amount", "DisplayOrder"]);
}
