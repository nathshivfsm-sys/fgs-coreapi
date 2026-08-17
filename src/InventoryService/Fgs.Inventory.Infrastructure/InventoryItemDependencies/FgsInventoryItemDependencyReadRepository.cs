using Fgs.Inventory.Application.Abstractions.InventoryItemDependencies;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.InventoryItemDependencies;

public sealed class FgsInventoryItemDependencyReadRepository(
    FgsInventoryDbContext context,
    ITenantContextAccessor tenantContextAccessor) : IFgsInventoryItemDependencyReadRepository
{
    public async Task<FgsInventoryItemDependencyDetailDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = ResolveScope();
        var entity = await context.FgsInventoryItemDependencies
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == id && x.TenantId == tenantId && x.CompanyId == companyId,
                cancellationToken);

        return entity is null ? null : Map(entity);
    }

    public async Task<IReadOnlyList<FgsInventoryItemDependencyDetailDto>> ListByInventoryItemIdAsync(
        long inventoryItemId,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = ResolveScope();
        var rows = await context.FgsInventoryItemDependencies
            .AsNoTracking()
            .Where(x =>
                x.InventoryItemId == inventoryItemId
                && x.TenantId == tenantId
                && x.CompanyId == companyId)
            .OrderBy(x => x.DisplayOrder)
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

    private static FgsInventoryItemDependencyDetailDto Map(Domain.Entities.FgsInventoryItemDependency entity) =>
        new(
            entity.Id,
            entity.DependentInventoryItemId,
            entity.Quantity,
            entity.IsRequired,
            entity.Notes,
            entity.DisplayOrder,
            entity.IsActive);
}
