using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Descriptors;

public static class FgsVendorDescriptor
{
    public static CatalogEntityDescriptor Create() => new(
        Key: EntityKeys.Vendor,
        EntityName: "FgsVendor",
        ClrType: typeof(Fgs.Setup.Domain.Entities.FgsVendor),
        SummaryDtoType: typeof(FgsVendorSummaryDto),
        DetailDtoType: typeof(FgsVendorDetailDto),
        CreateDtoType: typeof(FgsVendorCreateDto),
        UpdateDtoType: typeof(FgsVendorUpdateDto),
        PatchDtoType: typeof(FgsVendorPatchDto),
        TableName: "FgsVendor",
        Schema: "setup",
        KeyType: CatalogEntityKeyType.Long,
        Variant: CatalogEntityVariant.StandardLong,
        RoutePlural: "vendors",
        SwaggerTag: "Setup - Vendors",
        TableComment: "FgsVendor",
        SupportsSoftDelete: true,
        Columns:
        [
            new CatalogEntityColumnDescriptor(
                "Id", "Id", typeof(long), false, null, true, false, true, "Allowed values: VENDOR, SUBCONTRACTOR"),
            new CatalogEntityColumnDescriptor(
                "TenantId", "TenantId", typeof(long), false, null, true, false, false, "TenantId"),
            new CatalogEntityColumnDescriptor(
                "CompanyId", "CompanyId", typeof(long), false, null, true, false, false, "CompanyId"),
            new CatalogEntityColumnDescriptor(
                "VendorCode", "VendorCode", typeof(string), false, 0, false, true, true, "VendorCode"),
            new CatalogEntityColumnDescriptor(
                "Name", "Name", typeof(string), false, 200, false, true, true, "Name"),
            new CatalogEntityColumnDescriptor(
                "LegalName", "LegalName", typeof(string), false, 200, false, true, true, "LegalName"),
            new CatalogEntityColumnDescriptor(
                "VendorType", "VendorType", typeof(string), false, 50, false, true, true, "VendorType"),
            new CatalogEntityColumnDescriptor(
                "PaymentTermId", "PaymentTermId", typeof(long?), false, null, false, false, true, "PaymentTermId"),
            new CatalogEntityColumnDescriptor(
                "Email", "Email", typeof(string), false, 255, false, true, true, "Indicates whether vendor should be included in 1099 reporting."),
            new CatalogEntityColumnDescriptor(
                "PhoneNumber", "PhoneNumber", typeof(string), false, 50, false, true, true, "PhoneNumber"),
            new CatalogEntityColumnDescriptor(
                "MobileNumber", "MobileNumber", typeof(string), false, 50, false, true, true, "MobileNumber"),
            new CatalogEntityColumnDescriptor(
                "Website", "Website", typeof(string), false, 255, false, true, true, "Website"),
            new CatalogEntityColumnDescriptor(
                "TaxIdentificationNumber", "TaxIdentificationNumber", typeof(string), false, 100, false, true, true, "TaxIdentificationNumber"),
            new CatalogEntityColumnDescriptor(
                "LicenseNumber", "LicenseNumber", typeof(string), false, 100, false, true, true, "LicenseNumber"),
            new CatalogEntityColumnDescriptor(
                "InsurancePolicyNumber", "InsurancePolicyNumber", typeof(string), false, 100, false, true, true, "InsurancePolicyNumber"),
            new CatalogEntityColumnDescriptor(
                "Notes", "Notes", typeof(string), false, 0, false, true, true, "Notes"),
            new CatalogEntityColumnDescriptor(
                "Is1099Eligible", "Is1099Eligible", typeof(bool), true, null, false, false, true, "Is1099Eligible"),
            new CatalogEntityColumnDescriptor(
                "CreatedOn", "CreatedOn", typeof(DateTimeOffset), false, null, true, false, false, "CreatedOn"),
            new CatalogEntityColumnDescriptor(
                "CreatedBy", "CreatedBy", typeof(string), false, 0, true, false, false, "CreatedBy"),
            new CatalogEntityColumnDescriptor(
                "UpdatedOn", "UpdatedOn", typeof(DateTimeOffset?), false, null, true, false, false, "UpdatedOn"),
            new CatalogEntityColumnDescriptor(
                "UpdatedBy", "UpdatedBy", typeof(string), false, 0, true, false, false, "UpdatedBy"),
            new CatalogEntityColumnDescriptor(
                "IsActive", "IsActive", typeof(bool), true, null, false, false, true, "References payment terms used for accounts payable due date calculation."),
        ],
        UniqueKeys:
        [
            new CatalogEntityUniqueKeyDescriptor("UQ_FgsVendor", ["TenantId", "CompanyId", "VendorCode"]),
        ],
        SearchableColumns: ["VendorCode", "Name", "LegalName", "VendorType", "Email", "PhoneNumber", "MobileNumber", "Website", "TaxIdentificationNumber", "LicenseNumber", "InsurancePolicyNumber", "Notes"],
        SortableColumns: ["Id", "VendorCode", "Name", "LegalName", "VendorType", "PaymentTermId", "Email", "PhoneNumber", "MobileNumber", "Website", "TaxIdentificationNumber", "LicenseNumber", "InsurancePolicyNumber", "Notes", "Is1099Eligible", "IsActive"]);
}
