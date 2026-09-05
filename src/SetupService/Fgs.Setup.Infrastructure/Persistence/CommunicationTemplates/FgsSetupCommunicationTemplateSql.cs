using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.CommunicationTemplates;

internal static class FgsSetupCommunicationTemplateSql
{
    public const string Table = "setup.\"FgsSetupCommunicationTemplate\"";

    public const string SelectDetailColumns = """
        "Id", "CommunicationChannel", "TemplateType", "Code", "Name", "Subject", "Body", "IsMobileVisible", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "CommunicationChannel", "TemplateType", "Code", "Name", "Subject", "Body", "IsMobileVisible", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "CommunicationChannel", "TemplateType", "Code", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "CommunicationChannel", "TemplateType", "Code", "Name", "Subject", "Body", "IsMobileVisible"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns);

}