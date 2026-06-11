namespace Fgs.CatalogCrud.CodeGen;

internal sealed record CodeGenOptions
{
    public required string Service { get; init; }

    public required string InfrastructurePath { get; init; }

    public required string ApplicationPath { get; init; }

    public required string ApiPath { get; init; }

    public required string ApplicationNamespace { get; init; }

    public required string ApiNamespace { get; init; }

    public required string DomainProjectPath { get; init; }

    public required string EntityNamespace { get; init; }

    public string EntityNamePrefix { get; init; } = "Fgs";

    public string DefaultSchema { get; init; } = "setup";

    public string? EntityFilter { get; init; }

    public bool DryRun { get; init; }

    public HashSet<string> ExcludedEntities { get; init; } = [];

    public Func<Type, CatalogEntityVariant> ResolveVariant { get; init; } = DefaultResolveVariant;

    public Func<string, string> ResolveSwaggerTag { get; init; } = _ => "Catalog";

    public static CatalogEntityVariant DefaultResolveVariant(Type entityType) =>
        entityType.GetProperty("Id")?.PropertyType == typeof(Guid)
            ? CatalogEntityVariant.StandardGuid
            : CatalogEntityVariant.StandardLong;
}
