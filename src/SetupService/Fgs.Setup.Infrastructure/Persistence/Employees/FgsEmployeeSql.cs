using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.Employees;

internal static class FgsEmployeeSql
{
    public const string Table = "setup.\"FgsEmployee\"";

    public const string TechnicianProfileTable = "setup.\"FgsEmployeeTechnicianProfile\"";

    public const string SelectDetailColumns = """
        e."Id", e."UserId", e."EmployeeNumber", e."EmployeeTypeId", e."DisplayName", e."LegalFirstName", e."LegalMiddleName", e."LegalLastName", e."BirthDate", e."HireDate", e."TerminationDate", e."StatusId", e."PersonalEmail", e."OfficeEmail", e."PersonalPhone", e."OfficePhone", e."AddressId", e."ProfilePhotoFileId", e."RegularRate", e."OvertimeRate", e."DoubleTimeRate", e."LaborBurdenTypeId", e."LaborBurdenValue", e."IsPurchaser", e."Notes",
        loc."Id" AS "LocationId", loc."AddressLine1", loc."AddressLine2", loc."City", loc."State", loc."Country", loc."PostalCode",
        tp."Id" AS "TechnicianProfileId", tp."TechCode", tp."TechName", tp."CanBeScheduled", tp."DailyCapacityHours", tp."DispatchZoneId", tp."StartLocationTypeId", tp."StartTime", tp."TechTradeId", tp."TechSkillId", tp."TruckId", tp."CustomerFacingPhone" AS "TechnicianCustomerFacingPhone", tp."Notes" AS "TechnicianNotes"
        """;

    public const string SelectSummaryColumns = """
        e."Id", e."UserId", e."EmployeeNumber", e."EmployeeTypeId", e."DisplayName", e."LegalFirstName", e."LegalMiddleName", e."LegalLastName", e."BirthDate", e."HireDate", e."TerminationDate", e."StatusId", e."PersonalEmail", e."OfficeEmail", e."PersonalPhone", e."OfficePhone", e."ProfilePhotoFileId", e."RegularRate", e."OvertimeRate", e."DoubleTimeRate", e."LaborBurdenTypeId", e."LaborBurdenValue", e."IsPurchaser", e."Notes",
        (tp."Id" IS NOT NULL) AS "HasTechnicianProfile"
        """;

    public const string SelectLookupColumns = """
        "Id", "EmployeeNumber", "DisplayName"
        """;

    public const string LocationJoin = """
        LEFT JOIN setup."FgsLocation" loc
          ON loc."Id" = e."AddressId" AND loc."IsActive" = TRUE
        """;

    public const string TechnicianProfileJoin = """
        LEFT JOIN setup."FgsEmployeeTechnicianProfile" tp
          ON tp."EmployeeId" = e."Id"
         AND tp."TenantId" = e."TenantId"
         AND tp."CompanyId" = e."CompanyId"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "EmployeeNumber", "EmployeeTypeId", "DisplayName", "LegalFirstName", "LegalLastName",
        "HireDate", "StatusId", "OfficeEmail", "OfficePhone", "RegularRate", "CreatedOn"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            defaultColumn: "DisplayName",
            tableAlias: "e");
}
