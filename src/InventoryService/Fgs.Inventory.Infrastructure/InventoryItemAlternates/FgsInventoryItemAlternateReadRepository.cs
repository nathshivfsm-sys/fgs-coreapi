using Dapper;
using Fgs.Inventory.Application.Abstractions.InventoryItemAlternates;
using Fgs.Inventory.Application.Abstractions.Persistence;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.InventoryItems;
using Fgs.MultiTenancy;

namespace Fgs.Inventory.Infrastructure.InventoryItemAlternates;

public sealed class FgsInventoryItemAlternateReadRepository(
    IInventoryReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsInventoryItemAlternateReadRepository
{
    public async Task<FgsInventoryItemAlternateDetailDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsInventoryItemSql.SelectAlternateColumns}
            FROM {FgsInventoryItemSql.AlternateTable}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsInventoryItemAlternateRow>(
            new CommandDefinition(
                sql,
                new { Id = id, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));
        return row?.ToDto();
    }

    public async Task<IReadOnlyList<FgsInventoryItemAlternateDetailDto>> ListByInventoryItemIdAsync(
        long inventoryItemId,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsInventoryItemSql.SelectAlternateColumns}
            FROM {FgsInventoryItemSql.AlternateTable}
            WHERE "InventoryItemId" = @InventoryItemId
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            ORDER BY "PriorityOrder" ASC, "Id" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsInventoryItemAlternateRow>(
            new CommandDefinition(
                sql,
                new { InventoryItemId = inventoryItemId, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));
        return rows.Select(row => row.ToDto()).ToList();
    }
}
