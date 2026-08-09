using Fgs.Inventory.Application.Abstractions.VendorInventoryItems;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.VendorInventoryItems;

public sealed class FgsVendorInventoryItemWriteService : IFgsVendorInventoryItemWriteService
{
    private readonly FgsInventoryDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryEntityAuditHelper _auditHelper;

    public FgsVendorInventoryItemWriteService(
        FgsInventoryDbContext context,
        IUnitOfWork unitOfWork,
        InventoryEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsVendorInventoryItemDetailDto> CreateAsync(
        FgsVendorInventoryItemCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = MapCreate(dto);
        _auditHelper.StampForCreate(entity);
        await _context.FgsVendorInventoryItems.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsVendorInventoryItemDetailDto> UpdateAsync(
        long id,
        FgsVendorInventoryItemUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Vendor inventory item '{id}' was not found.");

        ApplyMutableFields(entity, dto);
        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsVendorInventoryItemDetailDto> PatchAsync(
        long id,
        FgsVendorInventoryItemPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Vendor inventory item '{id}' was not found.");

        if (dto.VendorId.HasValue)
        {
            entity.VendorId = dto.VendorId.Value;
        }

        if (dto.InventoryItemId.HasValue)
        {
            entity.InventoryItemId = dto.InventoryItemId.Value;
        }

        if (dto.VendorPartNumber is not null)
        {
            entity.VendorPartNumber = TrimOrNull(dto.VendorPartNumber);
        }

        if (dto.VendorPartName is not null)
        {
            entity.VendorPartName = TrimOrNull(dto.VendorPartName);
        }

        if (dto.LastCost.HasValue)
        {
            entity.LastCost = dto.LastCost.Value;
        }

        if (dto.LastReceivedDate.HasValue)
        {
            entity.LastReceivedDate = dto.LastReceivedDate;
        }

        if (dto.PurchaseOrderComments is not null)
        {
            entity.PurchaseOrderComments = TrimOrNull(dto.PurchaseOrderComments);
        }

        if (dto.VendorPriority.HasValue)
        {
            entity.VendorPriority = dto.VendorPriority.Value;
        }

        if (dto.LeadTimeDays.HasValue)
        {
            entity.LeadTimeDays = dto.LeadTimeDays;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsVendorInventoryItem?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsVendorInventoryItems.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A vendor inventory item for this vendor and item already exists.", ex);
        }
    }

    private static FgsVendorInventoryItem MapCreate(FgsVendorInventoryItemCreateDto dto) =>
        new()
        {
            VendorId = dto.VendorId,
            InventoryItemId = dto.InventoryItemId,
            VendorPartNumber = TrimOrNull(dto.VendorPartNumber),
            VendorPartName = TrimOrNull(dto.VendorPartName),
            LastCost = dto.LastCost,
            LastReceivedDate = dto.LastReceivedDate,
            PurchaseOrderComments = TrimOrNull(dto.PurchaseOrderComments),
            VendorPriority = dto.VendorPriority,
            LeadTimeDays = dto.LeadTimeDays
        };

    private static void ApplyMutableFields(FgsVendorInventoryItem entity, FgsVendorInventoryItemUpdateDto dto)
    {
        entity.VendorId = dto.VendorId;
        entity.InventoryItemId = dto.InventoryItemId;
        entity.VendorPartNumber = TrimOrNull(dto.VendorPartNumber);
        entity.VendorPartName = TrimOrNull(dto.VendorPartName);
        entity.LastCost = dto.LastCost;
        entity.LastReceivedDate = dto.LastReceivedDate;
        entity.PurchaseOrderComments = TrimOrNull(dto.PurchaseOrderComments);
        entity.VendorPriority = dto.VendorPriority;
        entity.LeadTimeDays = dto.LeadTimeDays;
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static FgsVendorInventoryItemDetailDto MapToDetail(FgsVendorInventoryItem entity) =>
        new(
            entity.Id,
            entity.VendorId,
            entity.InventoryItemId,
            entity.VendorPartNumber,
            entity.VendorPartName,
            entity.LastCost,
            entity.LastReceivedDate,
            entity.PurchaseOrderComments,
            entity.VendorPriority,
            entity.LeadTimeDays,
            entity.IsActive);
}
