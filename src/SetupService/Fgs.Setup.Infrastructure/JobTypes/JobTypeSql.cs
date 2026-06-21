using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.JobTypes;

internal static class JobTypeSql
{
    public const string Table = "setup.\"FgsJobType\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "JobTypeCategoryId", "JobTypeSubCategoryId", "JobTypeCode", "TaskName", "Description", "UsedFor", "Trade", "EstimatedDurationMinutes", "BusinessUnit", "Priority", "BackgroundColor", "TextColor", "ShowToFieldTech", "ShowOnCustomerPortal", "DisplayOrder", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "JobTypeCategoryId", "JobTypeSubCategoryId", "JobTypeCode", "TaskName", "Description", "UsedFor", "Trade", "EstimatedDurationMinutes", "BusinessUnit", "Priority", "BackgroundColor", "TextColor", "ShowToFieldTech", "ShowOnCustomerPortal", "DisplayOrder", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "JobTypeCode", "TaskName", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "DisplayOrder", "JobTypeCategoryId", "JobTypeSubCategoryId", "JobTypeCode", "TaskName", "Description", "UsedFor", "Trade", "EstimatedDurationMinutes", "BusinessUnit", "Priority", "BackgroundColor", "TextColor", "ShowToFieldTech", "ShowOnCustomerPortal"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"TaskName\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"TaskName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
