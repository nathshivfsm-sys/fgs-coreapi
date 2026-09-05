using Fgs.Inventory.Application.Abstractions.InventoryItemDependencies;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.InventoryItemDependencies;

public sealed class FgsInventoryItemDependencyWriteService(
    FgsInventoryDbContext context,
    IUnitOfWork unitOfWork,
    InventoryEntityAuditHelper auditHelper) : IFgsInventoryItemDependencyWriteService
{
    public async Task<IReadOnlyList<FgsInventoryItemDependencyDetailDto>> ReplaceAsync(
        FgsInventoryItemDependencyReplaceDto dto,
        CancellationToken cancellationToken = default)
    {
        var item = await context.FgsInventoryItems
            .Include(i => i.Dependencies)
            .FirstOrDefaultAsync(i => i.Id == dto.InventoryItemId, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory item '{dto.InventoryItemId}' was not found.");

        InventoryChildCollectionSync.Sync(
            context,
            item.Dependencies,
            dto.Items ?? [],
            row => row.Id,
            _ => new FgsInventoryItemDependency { InventoryItemId = item.Id },
            (entity, row, _) =>
            {
                entity.DependentInventoryItemId = row.DependentInventoryItemId;
                entity.Quantity = row.Quantity;
                entity.IsRequired = row.IsRequired;
                entity.Notes = TrimOrNull(row.Notes);
                entity.DisplayOrder = row.DisplayOrder;
                entity.IsActive = row.IsActive;
            },
            entity => auditHelper.StampForCreate(entity, entity),
            auditHelper.StampForUpdate,
            $"Inventory item dependency '{{0}}' was not found on inventory item '{item.Id}'.");

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "This dependent inventory item is already linked to the inventory item.",
                ex);
        }

        return item.Dependencies
            .OrderBy(d => d.DisplayOrder)
            .ThenBy(d => d.Id)
            .Select(d => new FgsInventoryItemDependencyDetailDto(
                d.Id,
                d.DependentInventoryItemId,
                d.Quantity,
                d.IsRequired,
                d.Notes,
                d.DisplayOrder,
                d.IsActive))
            .ToList();
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await context.FgsInventoryItemDependencies
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory item dependency '{id}' was not found.");

        context.FgsInventoryItemDependencies.Remove(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;
}
