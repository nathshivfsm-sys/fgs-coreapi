using Fgs.Inventory.Application.Abstractions.InventoryStocks;
using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.InventoryStocks;

public sealed class FgsInventoryStockWriteService : IFgsInventoryStockWriteService
{
    private readonly FgsInventoryDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryEntityAuditHelper _auditHelper;

    public FgsInventoryStockWriteService(
        FgsInventoryDbContext context,
        IUnitOfWork unitOfWork,
        InventoryEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsInventoryStockDetailDto> CreateAsync(
        FgsInventoryStockCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = MapCreate(dto);
        _auditHelper.StampForCreateStock(entity);
        await _context.FgsInventoryStocks.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsInventoryStockDetailDto> UpdateAsync(
        long id,
        FgsInventoryStockUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory stock '{id}' was not found.");

        ApplyMutableFields(entity, dto);
        _auditHelper.StampStockUpdated(entity);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsInventoryStockDetailDto> PatchAsync(
        long id,
        FgsInventoryStockPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory stock '{id}' was not found.");

        if (dto.InventoryItemId.HasValue)
        {
            entity.InventoryItemId = dto.InventoryItemId.Value;
        }

        if (dto.QuantityOnHand.HasValue)
        {
            entity.QuantityOnHand = dto.QuantityOnHand.Value;
        }

        if (dto.QuantityCommitted.HasValue)
        {
            entity.QuantityCommitted = dto.QuantityCommitted.Value;
        }

        if (dto.QuantityAvailable.HasValue)
        {
            entity.QuantityAvailable = dto.QuantityAvailable.Value;
        }

        if (dto.AverageCost.HasValue)
        {
            entity.AverageCost = dto.AverageCost.Value;
        }

        if (dto.LastCost.HasValue)
        {
            entity.LastCost = dto.LastCost.Value;
        }

        if (dto.LastPurchaseDate.HasValue)
        {
            entity.LastPurchaseDate = dto.LastPurchaseDate;
        }

        if (dto.LastSoldDate.HasValue)
        {
            entity.LastSoldDate = dto.LastSoldDate;
        }

        _auditHelper.StampStockUpdated(entity);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsInventoryStock?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsInventoryStocks.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("Inventory stock for this item already exists.", ex);
        }
    }

    private static FgsInventoryStock MapCreate(FgsInventoryStockCreateDto dto) =>
        new()
        {
            InventoryItemId = dto.InventoryItemId,
            QuantityOnHand = dto.QuantityOnHand,
            QuantityCommitted = dto.QuantityCommitted,
            QuantityAvailable = dto.QuantityAvailable,
            AverageCost = dto.AverageCost,
            LastCost = dto.LastCost,
            LastPurchaseDate = dto.LastPurchaseDate,
            LastSoldDate = dto.LastSoldDate
        };

    private static void ApplyMutableFields(FgsInventoryStock entity, FgsInventoryStockUpdateDto dto)
    {
        entity.InventoryItemId = dto.InventoryItemId;
        entity.QuantityOnHand = dto.QuantityOnHand;
        entity.QuantityCommitted = dto.QuantityCommitted;
        entity.QuantityAvailable = dto.QuantityAvailable;
        entity.AverageCost = dto.AverageCost;
        entity.LastCost = dto.LastCost;
        entity.LastPurchaseDate = dto.LastPurchaseDate;
        entity.LastSoldDate = dto.LastSoldDate;
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static FgsInventoryStockDetailDto MapToDetail(FgsInventoryStock entity) =>
        new(
            entity.Id,
            entity.InventoryItemId,
            entity.QuantityOnHand,
            entity.QuantityCommitted,
            entity.QuantityAvailable,
            entity.AverageCost,
            entity.LastCost,
            entity.LastPurchaseDate,
            entity.LastSoldDate,
            entity.UpdatedOn);
}
