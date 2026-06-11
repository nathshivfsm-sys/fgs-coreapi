using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupTaxDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupTax,
        EntityName: "FgsSetupTax",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupTax),
        SummaryDtoType: typeof(FgsSetupTaxSummaryDto),
        DetailDtoType: typeof(FgsSetupTaxDetailDto),
        CreateDtoType: typeof(FgsSetupTaxCreateDto),
        UpdateDtoType: typeof(FgsSetupTaxUpdateDto),
        PatchDtoType: typeof(FgsSetupTaxPatchDto),
        TableName: "FgsSetupTax",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "taxs",
        SwaggerTag: "Setup - Tax",
        TableComment: "FgsSetupTax",
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
                "TaxCode", "TaxCode", typeof(string), false, 0, false, true, true, "TaxCode"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 0, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "IsExternalSystemRecord", "IsExternalSystemRecord", typeof(bool), true, null, false, false, true, "IsExternalSystemRecord"),
            new CatalogEntityColumnDescriptor(
                "ExternalSystemId", "ExternalSystemId", typeof(string), false, 0, false, true, true, "ExternalSystemId"),
            new CatalogEntityColumnDescriptor(
                "SyncToken", "SyncToken", typeof(string), false, 100, false, true, true, "SyncToken"),
            new CatalogEntityColumnDescriptor(
                "ShowTaxDetail", "ShowTaxDetail", typeof(bool), true, null, false, false, true, "ShowTaxDetail"),
            new CatalogEntityColumnDescriptor(
                "Description", "Description", typeof(string), false, 0, false, true, true, "Description"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsSetupTax", ["TenantId", "CompanyId", "TaxCode"]),
        ],
        SearchableColumns: ["TaxCode", "Name", "ExternalSystemId", "SyncToken", "Description"],
        SortableColumns: ["Id", "TaxCode", "Name", "IsExternalSystemRecord", "ExternalSystemId", "SyncToken", "ShowTaxDetail", "Description", "IsActive"]);
}
