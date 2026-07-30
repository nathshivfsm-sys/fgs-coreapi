using Fgs.Inventory.Application.Abstractions.TruckStockTemplateItems;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.TruckStockTemplateItems;

public sealed class FgsTruckStockTemplateItemWriteService : IFgsTruckStockTemplateItemWriteService
{
    private readonly FgsInventoryDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryEntityAuditHelper _auditHelper;

    public FgsTruckStockTemplateItemWriteService(
        FgsInventoryDbContext context,
        IUnitOfWork unitOfWork,
        InventoryEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsTruckStockTemplateItemDetailDto> CreateAsync(
        long templateId,
        FgsTruckStockTemplateItemCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        await EnsureTemplateExistsAsync(templateId, cancellationToken);

        var entity = new FgsTruckStockTemplateItem
        {
            TruckStockTemplateId = templateId,
            InventoryItemId = dto.InventoryItemId,
            TargetQuantity = dto.TargetQuantity,
            MinimumQuantity = dto.MinimumQuantity,
            DisplayOrder = dto.DisplayOrder
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsTruckStockTemplateItems.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsTruckStockTemplateItemDetailDto> UpdateAsync(
        long templateId,
        long itemId,
        FgsTruckStockTemplateItemUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(templateId, itemId, cancellationToken)
            ?? throw new KeyNotFoundException(
                $"Truck stock template item '{itemId}' was not found on template '{templateId}'.");

        entity.InventoryItemId = dto.InventoryItemId;
        entity.TargetQuantity = dto.TargetQuantity;
        entity.MinimumQuantity = dto.MinimumQuantity;
        entity.DisplayOrder = dto.DisplayOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsTruckStockTemplateItemDetailDto> PatchAsync(
        long templateId,
        long itemId,
        FgsTruckStockTemplateItemPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(templateId, itemId, cancellationToken)
            ?? throw new KeyNotFoundException(
                $"Truck stock template item '{itemId}' was not found on template '{templateId}'.");

        if (dto.InventoryItemId.HasValue)
        {
            entity.InventoryItemId = dto.InventoryItemId.Value;
        }

        if (dto.TargetQuantity.HasValue)
        {
            entity.TargetQuantity = dto.TargetQuantity.Value;
        }

        if (dto.MinimumQuantity.HasValue)
        {
            entity.MinimumQuantity = dto.MinimumQuantity.Value;
        }

        if (dto.DisplayOrder.HasValue)
        {
            entity.DisplayOrder = dto.DisplayOrder.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task DeleteAsync(long templateId, long itemId, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(templateId, itemId, cancellationToken)
            ?? throw new KeyNotFoundException(
                $"Truck stock template item '{itemId}' was not found on template '{templateId}'.");

        _context.FgsTruckStockTemplateItems.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureTemplateExistsAsync(long templateId, CancellationToken cancellationToken)
    {
        var exists = await _context.FgsTruckStockTemplates.AnyAsync(e => e.Id == templateId, cancellationToken);
        if (!exists)
        {
            throw new KeyNotFoundException($"Truck stock template '{templateId}' was not found.");
        }
    }

    private async Task<FgsTruckStockTemplateItem?> FindEntityAsync(
        long templateId,
        long itemId,
        CancellationToken cancellationToken) =>
        await _context.FgsTruckStockTemplateItems.FirstOrDefaultAsync(
            e => e.Id == itemId && e.TruckStockTemplateId == templateId,
            cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "This inventory item is already on the truck stock template.",
                ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static FgsTruckStockTemplateItemDetailDto MapToDetail(FgsTruckStockTemplateItem entity) =>
        new(
            entity.Id,
            entity.TruckStockTemplateId,
            entity.InventoryItemId,
            entity.TargetQuantity,
            entity.MinimumQuantity,
            entity.DisplayOrder);
}
