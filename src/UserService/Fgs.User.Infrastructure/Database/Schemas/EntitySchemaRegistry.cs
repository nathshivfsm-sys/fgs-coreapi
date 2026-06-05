using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Database.Schemas;

internal static class EntitySchemaRegistry
{
    private static readonly Dictionary<Type, string> EntitySchemas = BuildEntitySchemas();

    public static void ApplySchemas(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType is null)
            {
                continue;
            }

            if (!EntitySchemas.TryGetValue(entityType.ClrType, out var schema))
            {
                throw new InvalidOperationException(
                    $"No PostgreSQL schema mapping for entity '{entityType.ClrType.Name}'. " +
                    "Add it to EntitySchemaRegistry.");
            }

            entityType.SetSchema(schema);
        }
    }

    private static readonly Dictionary<string, string> TableSchemas = BuildTableSchemas();

    public static string GetSchemaForTable(string tableName) =>
        TableSchemas.TryGetValue(tableName, out var schema)
            ? schema
            : throw new InvalidOperationException(
                $"No PostgreSQL schema mapping for table '{tableName}'. Add it to EntitySchemaRegistry.");

    public static string QualifyTable(string tableName) =>
        $"{GetSchemaForTable(tableName)}.\"{tableName}\"";

    private static Dictionary<string, string> BuildTableSchemas() => new()
    {
        ["FgsTenant"] = FgsDatabaseSchemas.Tenant,
        ["FgsTenantCompany"] = FgsDatabaseSchemas.Tenant,
    };

    private static Dictionary<Type, string> BuildEntitySchemas() => new()
    {
        [typeof(FgsUser)] = FgsDatabaseSchemas.Identity,
        [typeof(FgsUserRole)] = FgsDatabaseSchemas.Identity,
        [typeof(FgsRole)] = FgsDatabaseSchemas.Identity,
        [typeof(FgsInvitation)] = FgsDatabaseSchemas.Identity,
        [typeof(FgsTenant)] = FgsDatabaseSchemas.Tenant,
        [typeof(FgsTenantCompany)] = FgsDatabaseSchemas.Tenant,
        [typeof(FgsTenantServiceSetup)] = FgsDatabaseSchemas.Tenant,
        [typeof(FgsLocation)] = FgsDatabaseSchemas.Tenant,
        [typeof(TenantOutboxMessage)] = FgsDatabaseSchemas.Tenant,
        [typeof(FgsTenantCompanyCache)] = FgsDatabaseSchemas.Identity,
    };
}
