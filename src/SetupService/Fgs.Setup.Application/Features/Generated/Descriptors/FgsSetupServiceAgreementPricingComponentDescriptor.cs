using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupServiceAgreementPricingComponentDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupServiceAgreementPricingComponent,
        EntityName: "FgsSetupServiceAgreementPricingComponent",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupServiceAgreementPricingComponent),
        SummaryDtoType: typeof(FgsSetupServiceAgreementPricingComponentSummaryDto),
        DetailDtoType: typeof(FgsSetupServiceAgreementPricingComponentDetailDto),
        CreateDtoType: typeof(FgsSetupServiceAgreementPricingComponentCreateDto),
        UpdateDtoType: typeof(FgsSetupServiceAgreementPricingComponentUpdateDto),
        PatchDtoType: typeof(FgsSetupServiceAgreementPricingComponentPatchDto),
        TableName: "FgsSetupServiceAgreementPricingComponent",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "serviceagreementpricingcomponents",
        SwaggerTag: "Setup - ServiceAgreements",
        TableComment: "FgsSetupServiceAgreementPricingComponent",
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
                "PricingComponentCode", "PricingComponentCode", typeof(string), false, 0, false, true, true, "PricingComponentCode"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 200, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "PricingComponentTypeCode", "PricingComponentTypeCode", typeof(string), false, 50, false, true, true, "PricingComponentTypeCode"),
            new CatalogEntityColumnDescriptor(
                "Amount", "Amount", typeof(decimal), true, null, false, false, true, "Amount"),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 0, false, true, true, "Description"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsSetupServiceAgreementPricingComponent", ["TenantId", "CompanyId", "PricingComponentCode"]),
        ],
        SearchableColumns: ["PricingComponentCode", "Name", "PricingComponentTypeCode", "Description"],
        SortableColumns: ["Id", "PricingComponentCode", "Name", "PricingComponentTypeCode", "Amount", "Description", "DisplayOrder", "IsActive"]);
}
