using Fgs.Inventory.Application.Abstractions.InventoryItemTypes;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.InventoryItemTypes;

public sealed class FgsInventoryItemTypeWriteService : IFgsInventoryItemTypeWriteService
{
    private readonly FgsInventoryDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryEntityAuditHelper _auditHelper;

    public FgsInventoryItemTypeWriteService(
        FgsInventoryDbContext context,
        IUnitOfWork unitOfWork,
        InventoryEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsInventoryItemTypeDetailDto> CreateAsync(
        FgsInventoryItemTypeCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsInventoryItemType
        {
            ItemTypeCode = NormalizeCode(dto.ItemTypeCode),
            Name = dto.Name.Trim(),
            Description = TrimOrNull(dto.Description),
            TracksQuantity = dto.TracksQuantity,
            DisplayOrder = dto.DisplayOrder
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsInventoryItemTypes.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsInventoryItemTypeDetailDto> UpdateAsync(
        long id,
        FgsInventoryItemTypeUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory item type '{id}' was not found.");

        entity.ItemTypeCode = NormalizeCode(dto.ItemTypeCode);
        entity.Name = dto.Name.Trim();
        entity.Description = TrimOrNull(dto.Description);
        entity.TracksQuantity = dto.TracksQuantity;
        entity.DisplayOrder = dto.DisplayOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsInventoryItemTypeDetailDto> PatchAsync(
        long id,
        FgsInventoryItemTypePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory item type '{id}' was not found.");

        if (dto.ItemTypeCode is not null) entity.ItemTypeCode = NormalizeCode(dto.ItemTypeCode);
        if (dto.Name is not null) entity.Name = dto.Name.Trim();
        if (dto.Description is not null) entity.Description = TrimOrNull(dto.Description);
        if (dto.TracksQuantity.HasValue) entity.TracksQuantity = dto.TracksQuantity.Value;
        if (dto.DisplayOrder.HasValue) entity.DisplayOrder = dto.DisplayOrder.Value;
        if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsInventoryItemType?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsInventoryItemTypes.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("An inventory item type with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();
    private static string? TrimOrNull(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static FgsInventoryItemTypeDetailDto MapToDetail(FgsInventoryItemType entity) =>
        new(entity.Id, entity.ItemTypeCode, entity.Name, entity.Description, entity.TracksQuantity, entity.DisplayOrder, entity.IsSystem, entity.IsActive);
}
