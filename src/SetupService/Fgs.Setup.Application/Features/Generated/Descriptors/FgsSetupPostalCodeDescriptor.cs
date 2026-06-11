using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsSetupPostalCodeDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.SetupPostalCode,
        EntityName: "FgsSetupPostalCode",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsSetupPostalCode),
        SummaryDtoType: typeof(FgsSetupPostalCodeSummaryDto),
        DetailDtoType: typeof(FgsSetupPostalCodeDetailDto),
        CreateDtoType: typeof(FgsSetupPostalCodeCreateDto),
        UpdateDtoType: typeof(FgsSetupPostalCodeUpdateDto),
        PatchDtoType: typeof(FgsSetupPostalCodePatchDto),
        TableName: "FgsSetupPostalCode",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "postalcodes",
        SwaggerTag: "Setup - Tax",
        TableComment: "FgsSetupPostalCode",
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
                "PostalCode", "PostalCode", typeof(string), false, 0, false, true, true, "PostalCode"),
            new CatalogEntityColumnDescriptor(
                "FgsSetupZoneId", "FgsSetupZoneId", typeof(long?), false, null, false, false, true, "FgsSetupZoneId"),
            new CatalogEntityColumnDescriptor(
                "FgsSetupTaxId", "FgsSetupTaxId", typeof(long?), false, null, false, false, true, "FgsSetupTaxId"),
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
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsSetupPostalCode", ["TenantId", "CompanyId", "PostalCode"]),
        ],
        SearchableColumns: ["PostalCode"],
        SortableColumns: ["Id", "PostalCode", "FgsSetupZoneId", "FgsSetupTaxId", "IsActive"]);
}
