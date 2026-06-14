using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupServiceAgreementTemplateCoverageDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupServiceAgreementTemplateCoverage,
        EntityName: "FgsSetupServiceAgreementTemplateCoverage",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupServiceAgreementTemplateCoverage),
        SummaryDtoType: typeof(FgsSetupServiceAgreementTemplateCoverageSummaryDto),
        DetailDtoType: typeof(FgsSetupServiceAgreementTemplateCoverageDetailDto),
        CreateDtoType: typeof(FgsSetupServiceAgreementTemplateCoverageCreateDto),
        UpdateDtoType: typeof(FgsSetupServiceAgreementTemplateCoverageUpdateDto),
        PatchDtoType: typeof(FgsSetupServiceAgreementTemplateCoveragePatchDto),
        TableName: "FgsSetupServiceAgreementTemplateCoverage",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "serviceagreementtemplatecoverages",
        SwaggerTag: "Setup - ServiceAgreements",
        TableComment: "FgsSetupServiceAgreementTemplateCoverage",
        SupportsSoftDelete: false,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Service agreement template that this coverage item belongs to."),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "TenantId"),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "CompanyId"),
            new CatalogEntityColumnDescriptor(
                "ServiceAgreementTemplateId", "ServiceAgreementTemplateId", typeof(long), true, null, false, false, true, "ServiceAgreementTemplateId"),
            new CatalogEntityColumnDescriptor(
                "CoverageTypeCode", "CoverageTypeCode", typeof(string), false, 0, false, true, true, "INCLUDE or EXCLUDE."),
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
        ],
        UniqueKeys:
        [
        ],
        SearchableColumns: ["CoverageTypeCode", "Description"],
        SortableColumns: ["Id", "ServiceAgreementTemplateId", "CoverageTypeCode", "Description", "DisplayOrder"]);
}
