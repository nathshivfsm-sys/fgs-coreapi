using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.Vendors;

internal static class FgsVendorSql
{
    public const string Table = "setup.\"FgsVendor\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "VendorCode", "Name", "LegalName", "VendorType", "PaymentTermId", "Email", "PhoneNumber", "MobileNumber", "Website", "TaxIdentificationNumber", "LicenseNumber", "InsurancePolicyNumber", "Notes", "Is1099Eligible", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "VendorCode", "Name", "LegalName", "VendorType", "PaymentTermId", "Email", "PhoneNumber", "MobileNumber", "Website", "TaxIdentificationNumber", "LicenseNumber", "InsurancePolicyNumber", "Notes", "Is1099Eligible", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "VendorCode", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "VendorCode", "Name", "LegalName", "VendorType", "PaymentTermId", "Email", "PhoneNumber", "MobileNumber", "Website", "TaxIdentificationNumber", "LicenseNumber", "InsurancePolicyNumber", "Notes", "Is1099Eligible"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Name\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"Name\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
