using Fgs.Foundation.Paging;

namespace Fgs.Inventory.Infrastructure.Vendors;

internal static class FgsVendorSql
{
    public const string Table = "inventory.\"FgsVendor\"";

    public const string SelectDetailColumns = """
        "Id", "VendorCode", "Name", "LegalName", "VendorType", "VendorStatus", "VendorAccountNumber", "PaymentTermId",
        "ContactName", "ContactTitle", "Email", "PurchaseOrderEmail", "PhoneNumber", "MobileNumber", "FaxNumber", "Website",
        "Address1", "Address2", "City", "StateProvince", "PostalCode", "Country",
        "TaxIdNumber", "LicenseNumber", "InsurancePolicyNumber", "Notes", "Is1099Eligible", "IsActive"
        """;

    public const string SelectSummaryColumns = SelectDetailColumns;

    public const string SelectLookupColumns = """
        "Id", "VendorCode", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "VendorCode", "Name", "LegalName", "VendorType", "VendorStatus", "VendorAccountNumber",
        "PaymentTermId", "ContactName", "Email", "PhoneNumber", "MobileNumber", "Website", "City", "TaxIdNumber",
        "LicenseNumber", "InsurancePolicyNumber", "Notes", "Is1099Eligible"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}