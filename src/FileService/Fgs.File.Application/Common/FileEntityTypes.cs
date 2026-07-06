using Fgs.File.Domain.Enums;

namespace Fgs.File.Application.Common;

public static class FileEntityTypes
{
    public static bool TryParse(string? value, out FileEntityType entityType) =>
        Enum.TryParse(value, ignoreCase: true, out entityType)
        && Enum.IsDefined(entityType);

    public static bool IsSupported(string? value) => TryParse(value, out _);

    public static string ToStorageValue(FileEntityType entityType) => entityType.ToString();

    public static bool RequiresMatchingCompanyContext(FileEntityType entityType) =>
        entityType == FileEntityType.Company;

    public static bool RouteValueMatchesStorage(string routeEntityType, string storedEntityType) =>
        string.Equals(routeEntityType, storedEntityType, StringComparison.OrdinalIgnoreCase);
}
