using Fgs.Inventory.Application.Abstractions.InventoryItems;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.InventoryItems;

public sealed class FgsInventoryItemWriteService : IFgsInventoryItemWriteService
{
    private readonly FgsInventoryDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryEntityAuditHelper _auditHelper;

    public FgsInventoryItemWriteService(
        FgsInventoryDbContext context,
        IUnitOfWork unitOfWork,
        InventoryEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsInventoryItemDetailDto> CreateAsync(
        FgsInventoryItemCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = MapCreateEntity(dto);
        _auditHelper.StampForCreate(entity);
        await _context.FgsInventoryItems.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsInventoryItemDetailDto> UpdateAsync(
        long id,
        FgsInventoryItemUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, includeChildren: true, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory item '{id}' was not found.");

        ApplyUpdate(entity, dto);
        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsInventoryItemDetailDto> PatchAsync(
        long id,
        FgsInventoryItemPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, includeChildren: true, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory item '{id}' was not found.");

        ApplyPatch(entity, dto);
        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsInventoryItem?> FindEntityAsync(
        long id,
        bool includeChildren,
        CancellationToken cancellationToken)
    {
        IQueryable<FgsInventoryItem> query = _context.FgsInventoryItems;
        if (includeChildren)
        {
            query = query
                .Include(e => e.Alternates)
                .Include(e => e.Dependencies);
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "An inventory item with the same code already exists.",
                ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static FgsInventoryItem MapCreateEntity(FgsInventoryItemCreateDto dto) =>
        new()
        {
            InventoryItemTypeId = dto.InventoryItemTypeId,
            InventoryCategoryId = dto.InventoryCategoryId,
            InventorySubCategoryId = dto.InventorySubCategoryId,
            ItemCode = NormalizeCode(dto.ItemCode),
            Name = dto.Name.Trim(),
            Description = TrimOrNull(dto.Description),
            PurchaseDescription = TrimOrNull(dto.PurchaseDescription),
            SalesDescription = TrimOrNull(dto.SalesDescription),
            ManufacturerPartNumber = TrimOrNull(dto.ManufacturerPartNumber),
            ManufacturerName = TrimOrNull(dto.ManufacturerName),
            Sku = TrimOrNull(dto.Sku),
            UPCCode = TrimOrNull(dto.UPCCode),
            UnitOfMeasure = TrimOrNull(dto.UnitOfMeasure),
            TracksInventory = dto.TracksInventory,
            IsSerialized = dto.IsSerialized,
            UnitCost = dto.UnitCost,
            StandardUnitCost = dto.StandardUnitCost,
            SalesPrice = dto.SalesPrice
        };

    private static void ApplyUpdate(FgsInventoryItem entity, FgsInventoryItemUpdateDto dto)
    {
        entity.InventoryItemTypeId = dto.InventoryItemTypeId;
        entity.InventoryCategoryId = dto.InventoryCategoryId;
        entity.InventorySubCategoryId = dto.InventorySubCategoryId;
        entity.ItemCode = NormalizeCode(dto.ItemCode);
        entity.Name = dto.Name.Trim();
        entity.Description = TrimOrNull(dto.Description);
        entity.PurchaseDescription = TrimOrNull(dto.PurchaseDescription);
        entity.SalesDescription = TrimOrNull(dto.SalesDescription);
        entity.ManufacturerPartNumber = TrimOrNull(dto.ManufacturerPartNumber);
        entity.ManufacturerName = TrimOrNull(dto.ManufacturerName);
        entity.Sku = TrimOrNull(dto.Sku);
        entity.UPCCode = TrimOrNull(dto.UPCCode);
        entity.UnitOfMeasure = TrimOrNull(dto.UnitOfMeasure);
        entity.TracksInventory = dto.TracksInventory;
        entity.IsSerialized = dto.IsSerialized;
        entity.UnitCost = dto.UnitCost;
        entity.StandardUnitCost = dto.StandardUnitCost;
        entity.SalesPrice = dto.SalesPrice;
    }

    private static void ApplyPatch(FgsInventoryItem entity, FgsInventoryItemPatchDto dto)
    {
        if (dto.InventoryItemTypeId.HasValue) entity.InventoryItemTypeId = dto.InventoryItemTypeId.Value;
        if (dto.InventoryCategoryId.HasValue) entity.InventoryCategoryId = dto.InventoryCategoryId;
        if (dto.InventorySubCategoryId.HasValue) entity.InventorySubCategoryId = dto.InventorySubCategoryId;
        if (dto.ItemCode is not null) entity.ItemCode = NormalizeCode(dto.ItemCode);
        if (dto.Name is not null) entity.Name = dto.Name.Trim();
        if (dto.Description is not null) entity.Description = TrimOrNull(dto.Description);
        if (dto.PurchaseDescription is not null) entity.PurchaseDescription = TrimOrNull(dto.PurchaseDescription);
        if (dto.SalesDescription is not null) entity.SalesDescription = TrimOrNull(dto.SalesDescription);
        if (dto.ManufacturerPartNumber is not null) entity.ManufacturerPartNumber = TrimOrNull(dto.ManufacturerPartNumber);
        if (dto.ManufacturerName is not null) entity.ManufacturerName = TrimOrNull(dto.ManufacturerName);
        if (dto.Sku is not null) entity.Sku = TrimOrNull(dto.Sku);
        if (dto.UPCCode is not null) entity.UPCCode = TrimOrNull(dto.UPCCode);
        if (dto.UnitOfMeasure is not null) entity.UnitOfMeasure = TrimOrNull(dto.UnitOfMeasure);
        if (dto.TracksInventory.HasValue) entity.TracksInventory = dto.TracksInventory.Value;
        if (dto.IsSerialized.HasValue) entity.IsSerialized = dto.IsSerialized.Value;
        if (dto.UnitCost.HasValue) entity.UnitCost = dto.UnitCost.Value;
        if (dto.StandardUnitCost.HasValue) entity.StandardUnitCost = dto.StandardUnitCost.Value;
        if (dto.SalesPrice.HasValue) entity.SalesPrice = dto.SalesPrice.Value;
        if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value;
    }

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static FgsInventoryItemDetailDto MapToDetail(FgsInventoryItem entity) =>
        new(
            entity.Id,
            entity.InventoryItemTypeId,
            entity.InventoryCategoryId,
            entity.InventorySubCategoryId,
            entity.ItemCode,
            entity.Name,
            entity.Description,
            entity.PurchaseDescription,
            entity.SalesDescription,
            entity.ManufacturerPartNumber,
            entity.ManufacturerName,
            entity.Sku,
            entity.UPCCode,
            entity.UnitOfMeasure,
            entity.TracksInventory,
            entity.IsSerialized,
            entity.UnitCost,
            entity.StandardUnitCost,
            entity.SalesPrice,
            entity.IsActive,
            entity.Alternates
                .OrderBy(a => a.PriorityOrder)
                .ThenBy(a => a.Id)
                .Select(a => new FgsInventoryItemAlternateDetailDto(
                    a.Id,
                    a.AlternateInventoryItemId,
                    a.PriorityOrder,
                    a.Notes,
                    a.IsActive))
                .ToList(),
            entity.Dependencies
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
                .ToList());
}
