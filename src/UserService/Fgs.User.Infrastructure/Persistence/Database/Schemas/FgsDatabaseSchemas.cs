namespace Fgs.User.Infrastructure.Persistence.Database.Schemas;

/// <summary>
/// PostgreSQL schema names aligned with FSM domain boundaries.
/// See .cursor/FSM_Recommended_Database_Schema_Structure.md
/// </summary>
public static class FgsDatabaseSchemas
{
    public const string Glo = "glo";
    public const string Identity = "identity";
    public const string Tenant = "tenant";
    public const string Setup = "setup";
    public const string Shared = "shared";
    public const string Audit = "audit";

    /// <summary>Schema for EF Core migration history.</summary>
    public const string MigrationHistory = Shared;

    public static readonly IReadOnlyList<string> All =
    [
        Glo,
        Identity,
        Tenant,
        Setup,
        Shared,
        Audit
    ];
}
