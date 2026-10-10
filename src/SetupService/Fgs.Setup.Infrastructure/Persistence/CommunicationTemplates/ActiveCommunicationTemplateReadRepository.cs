using Dapper;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Abstractions.Persistence;

namespace Fgs.Setup.Infrastructure.Persistence.CommunicationTemplates;

internal sealed class ActiveCommunicationTemplateReadRepository(ISetupReadConnectionFactory connectionFactory)
    : IActiveCommunicationTemplateReadRepository
{
    public async Task<IReadOnlyList<ActiveCommunicationTemplateCandidate>> ListFgsMatchesAsync(
        long? tenantId,
        long? companyId,
        string templateType,
        string code,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT "Id", "TenantId", "CompanyId", "TemplateType", "Code", "Name", "Subject", "Body", "IsMobileVisible", "IsActive"
            FROM setup."FgsSetupCommunicationTemplate"
            WHERE "TemplateType" = @TemplateType
              AND "Code" = @Code
              AND "IsActive" = TRUE
              AND (
                    (@CompanyId IS NOT NULL AND "TenantId" = @TenantId AND "CompanyId" = @CompanyId)
                    OR (@TenantId IS NOT NULL AND "TenantId" = @TenantId AND "CompanyId" IS NULL)
                    OR ("TenantId" IS NULL AND "CompanyId" IS NULL)
                  )
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<ActiveCommunicationTemplateCandidate>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, CompanyId = companyId, TemplateType = templateType, Code = code },
                cancellationToken: cancellationToken));
        return rows.ToList();
    }

    public async Task<ActiveCommunicationTemplateCandidate?> FindLatestGloAsync(
        string templateType,
        string communicationChannel,
        string code,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT "Id",
                   NULL::bigint AS "TenantId",
                   NULL::bigint AS "CompanyId",
                   @TemplateType AS "TemplateType",
                   "TemplateCode" AS "Code",
                   "Name",
                   "Subject",
                   "Body",
                   "IsMobileVisible",
                   "IsActive"
            FROM glo."GloCommunicationTemplate"
            WHERE "TemplateCode" = @Code
              AND "CommunicationChannel" = @CommunicationChannel
              AND "IsActive" = TRUE
            ORDER BY "Id" DESC
            LIMIT 1
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<ActiveCommunicationTemplateCandidate>(
            new CommandDefinition(
                sql,
                new { CommunicationChannel = communicationChannel, Code = code, TemplateType = templateType },
                cancellationToken: cancellationToken));
    }
}
