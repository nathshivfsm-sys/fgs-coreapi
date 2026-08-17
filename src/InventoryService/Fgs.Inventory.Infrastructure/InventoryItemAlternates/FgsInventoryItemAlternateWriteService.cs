using Fgs.Inventory.Application.Abstractions.InventoryItemAlternates;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.InventoryItemAlternates;

public sealed class FgsInventoryItemAlternateWriteService(
    FgsInventoryDbContext context,
    IUnitOfWork unitOfWork,
    InventoryEntityAuditHelper auditHelper) : IFgsInventoryItemAlternateWriteService
{
    public async Task<IReadOnlyList<FgsInventoryItemAlternateDetailDto>> ReplaceAsync(
        FgsInventoryItemAlternateReplaceDto dto,
        CancellationToken cancellationToken = default)
    {
        var item = await context.FgsInventoryItems
            .Include(i => i.Alternates)
            .FirstOrDefaultAsync(i => i.Id == dto.InventoryItemId, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory item '{dto.InventoryItemId}' was not found.");

        InventoryChildCollectionSync.Sync(
            context,
            item.Alternates,
            dto.Items ?? [],
            row => row.Id,
            _ => new FgsInventoryItemAlternate { InventoryItemId = item.Id },
            (entity, row, _) =>
            {
                entity.AlternateInventoryItemId = row.AlternateInventoryItemId;
                entity.PriorityOrder = row.PriorityOrder;
                entity.Notes = TrimOrNull(row.Notes);
                entity.IsActive = row.IsActive;
            },
            entity => auditHelper.StampForCreate(entity, entity),
            auditHelper.StampForUpdate,
            $"Inventory item alternate '{{0}}' was not found on inventory item '{item.Id}'.");

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "This alternate inventory item is already linked to the inventory item.",
                ex);
        }

        return item.Alternates
            .OrderBy(a => a.PriorityOrder)
            .ThenBy(a => a.Id)
            .Select(a => new FgsInventoryItemAlternateDetailDto(
                a.Id,
                a.AlternateInventoryItemId,
                a.PriorityOrder,
                a.Notes,
                a.IsActive))
            .ToList();
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await context.FgsInventoryItemAlternates
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory item alternate '{id}' was not found.");

        context.FgsInventoryItemAlternates.Remove(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;
}
