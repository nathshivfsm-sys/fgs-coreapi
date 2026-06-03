using Fgs.MultiTenancy.Constants;
using Fgs.User.Infrastructure.Persistence.Database.DbContexts;
using Fgs.User.Infrastructure.Persistence.Database.Schemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Infrastructure.Persistence.Database.Seed;

/// <summary>
/// Ensures sentinel tenant/company rows (Id 0) exist for platform-global credentials.
/// </summary>
public sealed class PlatformTenantSeeder(
    FgsUserDbContext dbContext,
    ILogger<PlatformTenantSeeder> logger)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await dbContext.FgsTenants.AnyAsync(t => t.Id == TenantScopeConstants.PlatformTenantId, cancellationToken))
        {
            return;
        }

        var tenantTable = EntitySchemaRegistry.QualifyTable("FgsTenant");
        var companyTable = EntitySchemaRegistry.QualifyTable("FgsTenantCompany");

        await dbContext.Database.ExecuteSqlRawAsync(
            BuildPlatformTenantInsertSql(tenantTable, TenantScopeConstants.PlatformTenantCode),
            cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync(
            BuildPlatformCompanyInsertSql(companyTable, TenantScopeConstants.PlatformTenantCode),
            cancellationToken);

        logger.LogInformation(
            "Platform sentinel tenant/company seeded (TenantId={TenantId}, CompanyNumber={CompanyId}).",
            TenantScopeConstants.PlatformTenantId,
            TenantScopeConstants.PlatformCompanyId);
    }

    private static string BuildPlatformTenantInsertSql(string tenantTable, string tenantCode) =>
        "INSERT INTO " + tenantTable +
        " (\"Id\", \"TenantCode\", \"Name\", \"FgsTenantStatusId\", \"IsActive\", \"CreatedOn\") " +
        "OVERRIDING SYSTEM VALUE " +
        "SELECT 0, '" + tenantCode + "', 'Platform Global', 3, true, timezone('utc', now()) " +
        "WHERE NOT EXISTS (SELECT 1 FROM " + tenantTable + " WHERE \"Id\" = 0);";

    private static string BuildPlatformCompanyInsertSql(string companyTable, string tenantCode) =>
        "INSERT INTO " + companyTable +
        " (\"TenantId\", \"CompanyGuid\", \"CompanyNumber\", \"BusinessTypeId\", \"Code\", \"Name\", \"IsActive\", \"CreatedOn\") " +
        "SELECT 0, '00000000-0000-0000-0000-000000000000'::uuid, 0, " +
        "(SELECT \"Id\" FROM glo.\"GloBusinessType\" ORDER BY \"Id\" LIMIT 1), " +
        "'" + tenantCode + "', 'Platform Global', true, timezone('utc', now()) " +
        "WHERE NOT EXISTS (SELECT 1 FROM " + companyTable + " WHERE \"TenantId\" = 0 AND \"CompanyNumber\" = 0);";
}
