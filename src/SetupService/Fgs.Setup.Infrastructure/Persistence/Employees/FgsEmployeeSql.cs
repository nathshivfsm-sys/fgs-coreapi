using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.Employees;

internal static class FgsEmployeeSql
{
    public const string Table = "setup.\"FgsEmployee\"";

    public const string SelectDetailColumns = """
        e."Id", e."UserId", e."EmployeeNumber", e."EmployeeTypeId", e."DisplayName", e."LegalFirstName", e."LegalMiddleName", e."LegalLastName", e."BirthDate", e."HireDate", e."TerminationDate", e."StatusId", e."PersonalEmail", e."OfficeEmail", e."PersonalPhone", e."OfficePhone", e."AddressId", e."ProfilePhotoFileId", e."RegularRate", e."OvertimeRate", e."DoubleTimeRate", e."LaborBurdenTypeId", e."LaborBurdenValue", e."IsPurchaser", e."Notes",
        loc."Id" AS "LocationId", loc."AddressLine1", loc."AddressLine2", loc."City", loc."State", loc."Country", loc."PostalCode"
        """;

    public const string SelectSummaryColumns = """
        "Id", "UserId", "EmployeeNumber", "EmployeeTypeId", "DisplayName", "LegalFirstName", "LegalMiddleName", "LegalLastName", "BirthDate", "HireDate", "TerminationDate", "StatusId", "PersonalEmail", "OfficeEmail", "PersonalPhone", "OfficePhone", "ProfilePhotoFileId", "RegularRate", "OvertimeRate", "DoubleTimeRate", "LaborBurdenTypeId", "LaborBurdenValue", "IsPurchaser", "Notes"
        """;

    public const string SelectLookupColumns = """
        "Id", "EmployeeNumber", "DisplayName"
        """;

    public const string LocationJoin = """
        LEFT JOIN setup."FgsLocation" loc
          ON loc."Id" = e."AddressId" AND loc."IsActive" = TRUE
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "EmployeeNumber", "EmployeeTypeId", "DisplayName", "LegalFirstName", "LegalLastName",
        "HireDate", "StatusId", "OfficeEmail", "OfficePhone", "RegularRate", "CreatedOn"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"DisplayName\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
