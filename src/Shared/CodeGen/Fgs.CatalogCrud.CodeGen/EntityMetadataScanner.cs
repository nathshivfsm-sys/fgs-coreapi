using System.Reflection;
using System.Text.RegularExpressions;

namespace Fgs.CatalogCrud.CodeGen;

internal static class EntityMetadataScanner
{
    private static readonly HashSet<string> ReadOnlyProperties =
    [
        "Id", "TenantId", "CompanyId", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
    ];

    public static IReadOnlyList<EntityMetadata> Scan(CodeGenOptions options)
    {
        var domainAssembly = LoadDomainAssembly(options.DomainProjectPath);
        var configComments = LoadConfigurationComments(options.InfrastructurePath);
        var configMaxLengths = LoadConfigurationMaxLengths(options.InfrastructurePath);
        var configUniqueKeys = LoadConfigurationUniqueKeys(options.InfrastructurePath);
        var configTableComments = LoadTableComments(options.InfrastructurePath);

        return domainAssembly
            .GetTypes()
            .Where(type => type.IsClass
                && !type.IsAbstract
                && type.Namespace == options.EntityNamespace
                && type.Name.StartsWith(options.EntityNamePrefix, StringComparison.Ordinal)
                && !options.ExcludedEntities.Contains(type.Name))
            .OrderBy(type => type.Name, StringComparer.Ordinal)
            .Select(type => BuildMetadata(type, options, configComments, configMaxLengths, configUniqueKeys, configTableComments))
            .ToList();
    }

    private static Assembly LoadDomainAssembly(string domainProjectPath)
    {
        var projectDir = Path.GetDirectoryName(domainProjectPath)!;
        var assemblyName = Path.GetFileNameWithoutExtension(domainProjectPath);
        var dllPath = Path.Combine(projectDir, "bin", "Debug", "net10.0", $"{assemblyName}.dll");

        if (!File.Exists(dllPath))
        {
            throw new FileNotFoundException(
                $"Domain assembly not found at '{dllPath}'. Build the domain project first: dotnet build \"{domainProjectPath}\"");
        }

        return Assembly.LoadFrom(dllPath);
    }

    private static EntityMetadata BuildMetadata(
        Type entityType,
        CodeGenOptions options,
        IReadOnlyDictionary<string, string> configComments,
        IReadOnlyDictionary<string, int> configMaxLengths,
        IReadOnlyDictionary<string, IReadOnlyList<string>> configUniqueKeys,
        IReadOnlyDictionary<string, string> configTableComments)
    {
        var variant = options.ResolveVariant(entityType);
        var keyType = entityType.GetProperty("Id")?.PropertyType == typeof(Guid)
            ? CatalogEntityKeyType.Guid
            : CatalogEntityKeyType.Long;

        var supportsSoftDelete = entityType.GetProperty("IsActive")?.PropertyType == typeof(bool)
            && variant != CatalogEntityVariant.HardDeleteScoped;

        var columns = entityType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => property.CanRead && IsMappedProperty(property))
            .OrderBy(property => GetPropertyOrder(property.Name))
            .ThenBy(property => property.MetadataToken)
            .Select(property =>
            {
                var commentKey = $"{entityType.Name}.{property.Name}";
                var isString = property.PropertyType == typeof(string);
                return new ColumnMetadata(
                    property.Name,
                    property.Name,
                    property.PropertyType,
                    IsRequired(property),
                    isString ? configMaxLengths.GetValueOrDefault(commentKey) : null,
                    ReadOnlyProperties.Contains(property.Name),
                    isString && !ReadOnlyProperties.Contains(property.Name),
                    !ReadOnlyProperties.Contains(property.Name) || property.Name is "Id" or "DisplayOrder" or "SortOrder",
                    SanitizeComment(configComments.GetValueOrDefault(commentKey) ?? property.Name));
            })
            .ToList();

        var uniqueKeys = configUniqueKeys.TryGetValue(entityType.Name, out var uniqueProperties)
            ? (IReadOnlyList<UniqueKeyMetadata>)[new UniqueKeyMetadata($"UQ_{entityType.Name}", uniqueProperties)]
            : [];

        return new EntityMetadata(
            entityType.Name,
            ToEntityKey(entityType.Name, options.EntityNamePrefix),
            ToRoutePlural(entityType.Name, options.EntityNamePrefix),
            options.ResolveSwaggerTag(entityType.Name),
            configTableComments.GetValueOrDefault(entityType.Name),
            entityType,
            variant,
            keyType,
            supportsSoftDelete,
            columns,
            uniqueKeys);
    }

    private static bool IsRequired(PropertyInfo property)
    {
        if (ReadOnlyProperties.Contains(property.Name))
        {
            return false;
        }

        if (Nullable.GetUnderlyingType(property.PropertyType) is not null)
        {
            return false;
        }

        return property.PropertyType != typeof(string)
            || property.GetCustomAttribute<System.ComponentModel.DataAnnotations.RequiredAttribute>() is not null;
    }

    private static string ToEntityKey(string entityName, string prefix) =>
        entityName.StartsWith(prefix, StringComparison.Ordinal)
            ? entityName[prefix.Length..]
            : entityName;

    private static string ToRoutePlural(string entityName, string prefix)
    {
        var withoutPrefix = entityName.StartsWith(prefix, StringComparison.Ordinal)
            ? entityName[prefix.Length..]
            : entityName;

        var words = Regex.Matches(withoutPrefix, "[A-Z][a-z0-9]*")
            .Select(match => match.Value.ToLowerInvariant())
            .ToList();

        if (words.Count > 1 && words[0] == "setup")
        {
            words.RemoveAt(0);
        }

        var last = words[^1];
        words[^1] = Pluralize(last);
        return string.Concat(words);
    }

    private static string Pluralize(string word) =>
        word switch
        {
            "category" => "categories",
            "company" => "companies",
            "inventory" => "inventories",
            "property" => "properties",
            _ when word.EndsWith("y", StringComparison.Ordinal) => word[..^1] + "ies",
            _ when word.EndsWith("s", StringComparison.Ordinal) => word + "es",
            _ => word + "s"
        };

    private static bool IsMappedProperty(PropertyInfo property)
    {
        if (property.GetIndexParameters().Length > 0)
        {
            return false;
        }

        var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
        return type == typeof(string)
            || type.IsPrimitive
            || type == typeof(decimal)
            || type == typeof(DateTime)
            || type == typeof(DateTimeOffset)
            || type == typeof(DateOnly)
            || type == typeof(Guid);
    }

    private static int GetPropertyOrder(string propertyName) =>
        propertyName switch
        {
            "Id" => 0,
            "TenantId" => 1,
            "CompanyId" => 2,
            "CreatedOn" => 900,
            "CreatedBy" => 901,
            "UpdatedOn" => 902,
            "UpdatedBy" => 903,
            "IsActive" => 904,
            _ => 100
        };

    private static string? SanitizeComment(string? comment) =>
        comment?.Replace('`', '\'').Replace("\"", "'");

    private static Dictionary<string, string> LoadConfigurationComments(string infrastructurePath)
    {
        var comments = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var file in Directory.GetFiles(Path.Combine(infrastructurePath, "Database", "Configurations"), "*Configuration.cs"))
        {
            var text = File.ReadAllText(file);
            foreach (Match match in Regex.Matches(text, @"entity\.Property\(e => e\.(\w+)\)[\s\S]*?\.HasComment\(""([^""]+)""\)"))
            {
                var entityName = GetEntityNameFromConfigFile(file);
                comments[$"{entityName}.{match.Groups[1].Value}"] = match.Groups[2].Value;
            }
        }

        return comments;
    }

    private static Dictionary<string, int> LoadConfigurationMaxLengths(string infrastructurePath)
    {
        var maxLengths = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var file in Directory.GetFiles(Path.Combine(infrastructurePath, "Database", "Configurations"), "*Configuration.cs"))
        {
            var text = File.ReadAllText(file);
            foreach (Match match in Regex.Matches(text, @"entity\.Property\(e => e\.(\w+)\)[\s\S]*?\.HasMaxLength\((\d+)\)"))
            {
                var entityName = GetEntityNameFromConfigFile(file);
                maxLengths[$"{entityName}.{match.Groups[1].Value}"] = int.Parse(match.Groups[2].Value);
            }
        }

        return maxLengths;
    }

    private static Dictionary<string, IReadOnlyList<string>> LoadConfigurationUniqueKeys(string infrastructurePath)
    {
        var uniqueKeys = new Dictionary<string, IReadOnlyList<string>>(StringComparer.Ordinal);
        foreach (var file in Directory.GetFiles(Path.Combine(infrastructurePath, "Database", "Configurations"), "*Configuration.cs"))
        {
            var text = File.ReadAllText(file);
            var match = Regex.Match(text, @"HasAlternateKey\(e => new \{ ([^}]+) \}\)");
            if (!match.Success)
            {
                continue;
            }

            var entityName = GetEntityNameFromConfigFile(file);
            var properties = match.Groups[1].Value
                .Split(',')
                .Select(value => value.Trim().Split('.').Last())
                .ToList();
            uniqueKeys[entityName] = properties;
        }

        return uniqueKeys;
    }

    private static Dictionary<string, string> LoadTableComments(string infrastructurePath)
    {
        var comments = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var file in Directory.GetFiles(Path.Combine(infrastructurePath, "Database", "Configurations"), "*Configuration.cs"))
        {
            var text = File.ReadAllText(file);
            var match = Regex.Match(text, @"ToTable\(""[^""]+"", t =>\s*t\.HasComment\(\s*""([^""]+)""");
            if (!match.Success)
            {
                continue;
            }

            var entityName = GetEntityNameFromConfigFile(file);
            comments[entityName] = match.Groups[1].Value;
        }

        return comments;
    }

    private static string GetEntityNameFromConfigFile(string file) =>
        Path.GetFileNameWithoutExtension(file).Replace("Configuration", string.Empty, StringComparison.Ordinal);
}
