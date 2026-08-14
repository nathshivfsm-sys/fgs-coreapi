using Fgs.Inventory.Application.Abstractions.InventoryItemAlternates;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.InventoryItemAlternates;

public sealed class FgsInventoryItemAlternateReadRepository(
    FgsInventoryDbContext context,
    ITenantContextAccessor tenantContextAccessor) : IFgsInventoryItemAlternateReadRepository
{
    public async Task<FgsInventoryItemAlternateDetailDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = ResolveScope();
        var entity = await context.FgsInventoryItemAlternates
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == id && x.TenantId == tenantId && x.CompanyId == companyId,
                cancellationToken);

        return entity is null ? null : Map(entity);
    }

    public async Task<IReadOnlyList<FgsInventoryItemAlternateDetailDto>> ListByInventoryItemIdAsync(
        long inventoryItemId,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = ResolveScope();
        var rows = await context.FgsInventoryItemAlternates
            .AsNoTracking()
            .Where(x =>
                x.InventoryItemId == inventoryItemId
                && x.TenantId == tenantId
                && x.CompanyId == companyId)
            .OrderBy(x => x.PriorityOrder)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return rows.Select(Map).ToList();
    }

    private (long TenantId, long CompanyId) ResolveScope()
    {
        var scope = tenantContextAccessor.Current
            ?? throw new InvalidOperationException("Tenant context is required.");
        return (scope.TenantId, scope.CompanyId);
    }

    private static FgsInventoryItemAlternateDetailDto Map(Domain.Entities.FgsInventoryItemAlternate entity) =>
        new(entity.Id, entity.AlternateInventoryItemId, entity.PriorityOrder, entity.Notes, entity.IsActive);
}
