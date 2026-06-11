using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupTaxDetailDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupTaxDetail,
        EntityName: "FgsSetupTaxDetail",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupTaxDetail),
        SummaryDtoType: typeof(FgsSetupTaxDetailSummaryDto),
        DetailDtoType: typeof(FgsSetupTaxDetailDetailDto),
        CreateDtoType: typeof(FgsSetupTaxDetailCreateDto),
        UpdateDtoType: typeof(FgsSetupTaxDetailUpdateDto),
        PatchDtoType: typeof(FgsSetupTaxDetailPatchDto),
        TableName: "FgsSetupTaxDetail",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "taxdetails",
        SwaggerTag: "Setup - Tax",
        TableComment: "FgsSetupTaxDetail",
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
                "FgsSetupTaxId", "FgsSetupTaxId", typeof(long), true, null, false, false, true, "FgsSetupTaxId"),
            new CatalogEntityColumnDescriptor(
                "FgsSetupTaxAuthorityId", "FgsSetupTaxAuthorityId", typeof(long), true, null, false, false, true, "FgsSetupTaxAuthorityId"),
            new CatalogEntityColumnDescriptor(
                "TaxPercent", "TaxPercent", typeof(decimal), true, null, false, false, true, "TaxPercent"),
            new CatalogEntityColumnDescriptor(
                "IsExternalSystemRecord", "IsExternalSystemRecord", typeof(bool), true, null, false, false, true, "IsExternalSystemRecord"),
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
        ],
        SearchableColumns: [],
        SortableColumns: ["Id", "FgsSetupTaxId", "FgsSetupTaxAuthorityId", "TaxPercent", "IsExternalSystemRecord", "IsActive"]);
}
