using Fgs.CatalogCrud.CodeGen;

var repoRoot = FindRepoRoot();
var options = ParseOptions(args, repoRoot);

if (string.IsNullOrWhiteSpace(options.InfrastructurePath)
    || string.IsNullOrWhiteSpace(options.ApplicationPath)
    || string.IsNullOrWhiteSpace(options.ApiPath)
    || string.IsNullOrWhiteSpace(options.DomainProjectPath))
{
    Console.Error.WriteLine("Missing required paths. Use --service Setup or specify all paths explicitly.");
    PrintUsage();
    return 1;
}

var entities = EntityMetadataScanner.Scan(options);
if (!string.IsNullOrWhiteSpace(options.EntityFilter))
{
    entities = entities
        .Where(entity => entity.EntityName.Equals(options.EntityFilter, StringComparison.OrdinalIgnoreCase)
            || entity.Key.Equals(options.EntityFilter, StringComparison.OrdinalIgnoreCase))
        .ToList();
}

Console.WriteLine($"Generating CRUD artifacts for {entities.Count} entities ({options.Service})...");
if (options.DryRun)
{
    foreach (var entity in entities)
    {
        Console.WriteLine($"- {entity.EntityName} -> /{entity.RoutePlural}");
    }

    return 0;
}

CodeGenerator.GenerateAll(entities, options);
Console.WriteLine("Generation complete.");
return 0;

static string FindRepoRoot()
{
    var current = new DirectoryInfo(AppContext.BaseDirectory);
    while (current is not null)
    {
        if (Directory.Exists(Path.Combine(current.FullName, "src", "SetupService")))
        {
            return current.FullName;
        }

        current = current.Parent;
    }

    throw new InvalidOperationException("Unable to locate repository root.");
}

static CodeGenOptions ParseOptions(string[] args, string repoRoot)
{
    var service = GetArgumentValue(args, "--service") ?? "Setup";
    var options = service.Equals("Setup", StringComparison.OrdinalIgnoreCase)
        ? CodeGenServiceProfiles.CreateSetupDefaults(repoRoot)
        : new CodeGenOptions
        {
            Service = service,
            InfrastructurePath = GetRequiredPath(args, "--infrastructure-path"),
            ApplicationPath = GetRequiredPath(args, "--application-path"),
            ApiPath = GetRequiredPath(args, "--api-path"),
            ApplicationNamespace = GetArgumentValue(args, "--application-namespace") ?? throw new InvalidOperationException("--application-namespace is required."),
            ApiNamespace = GetArgumentValue(args, "--api-namespace") ?? throw new InvalidOperationException("--api-namespace is required."),
            DomainProjectPath = GetRequiredPath(args, "--domain-project"),
            EntityNamespace = GetArgumentValue(args, "--entity-namespace") ?? throw new InvalidOperationException("--entity-namespace is required.")
        };

    options = options with
    {
        EntityFilter = GetArgumentValue(args, "--entity"),
        DryRun = args.Contains("--dry-run", StringComparer.OrdinalIgnoreCase),
        DefaultSchema = GetArgumentValue(args, "--default-schema") ?? options.DefaultSchema,
        ExcludedEntities = ParseExcludedEntities(GetArgumentValue(args, "--exclude")) is { Count: > 0 } excluded
            ? excluded
            : options.ExcludedEntities
    };

    var infrastructurePath = GetArgumentValue(args, "--infrastructure-path");
    if (!string.IsNullOrWhiteSpace(infrastructurePath))
    {
        options = options with { InfrastructurePath = Path.GetFullPath(infrastructurePath) };
    }

    var applicationPath = GetArgumentValue(args, "--application-path");
    if (!string.IsNullOrWhiteSpace(applicationPath))
    {
        options = options with { ApplicationPath = Path.GetFullPath(applicationPath) };
    }

    var apiPath = GetArgumentValue(args, "--api-path");
    if (!string.IsNullOrWhiteSpace(apiPath))
    {
        options = options with { ApiPath = Path.GetFullPath(apiPath) };
    }

    var domainProject = GetArgumentValue(args, "--domain-project");
    if (!string.IsNullOrWhiteSpace(domainProject))
    {
        options = options with { DomainProjectPath = Path.GetFullPath(domainProject) };
    }

    return options;
}

static HashSet<string> ParseExcludedEntities(string? value) =>
    string.IsNullOrWhiteSpace(value)
        ? []
        : value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToHashSet(StringComparer.Ordinal);

static string GetRequiredPath(string[] args, string name) =>
    GetArgumentValue(args, name) is { } value && !string.IsNullOrWhiteSpace(value)
        ? Path.GetFullPath(value)
        : string.Empty;

static string? GetArgumentValue(string[] args, string name)
{
    var index = Array.IndexOf(args, name);
    return index >= 0 && index + 1 < args.Length ? args[index + 1] : null;
}

static void PrintUsage()
{
    Console.WriteLine("""
        Usage:
          dotnet run --project src/Shared/CodeGen/Fgs.CatalogCrud.CodeGen -- --service Setup
          dotnet run --project src/Shared/CodeGen/Fgs.CatalogCrud.CodeGen -- \
            --service Inventory \
            --infrastructure-path <path> \
            --application-path <path> \
            --api-path <path> \
            --domain-project <path> \
            --application-namespace Fgs.Inventory.Application \
            --api-namespace Fgs.Inventory.API \
            --entity-namespace Fgs.Inventory.Domain.Entities \
            --default-schema inventory \
            --exclude Entity1,Entity2

        Options:
          --entity <name>     Generate a single entity
          --dry-run           List entities without writing files
          --exclude <list>    Comma-separated entity names to exclude
        """);
}
