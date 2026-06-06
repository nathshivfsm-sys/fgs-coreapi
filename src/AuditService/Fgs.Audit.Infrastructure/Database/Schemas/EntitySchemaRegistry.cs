using Fgs.Audit.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Audit.Infrastructure.Database.Schemas;

internal static class EntitySchemaRegistry
{
    public static void ApplySchemas(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType is null)
            {
                continue;
            }

            entityType.SetSchema(FgsDatabaseSchemas.Audit);
        }
    }
}
