using Fgs.Inventory.Application.Abstractions.InventorySerials;
using Fgs.Inventory.Application.Features.InventorySerials.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.InventorySerials;

public sealed class FgsInventorySerialWriteService : IFgsInventorySerialWriteService
{
    private readonly FgsInventoryDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryEntityAuditHelper _auditHelper;

    public FgsInventorySerialWriteService(
        FgsInventoryDbContext context,
        IUnitOfWork unitOfWork,
        InventoryEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsInventorySerialDetailDto> CreateAsync(
        FgsInventorySerialCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsInventorySerial
        {
            InventoryItemId = dto.InventoryItemId,
            SerialNumber = dto.SerialNumber.Trim(),
            InventorySerialStatus = dto.InventorySerialStatus,
            Notes = TrimOrNull(dto.Notes)
        };

        _auditHelper.StampForCreate(entity, entity);
        await _context.FgsInventorySerials.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsInventorySerialDetailDto> UpdateAsync(
        long id,
        FgsInventorySerialUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory serial '{id}' was not found.");

        entity.InventoryItemId = dto.InventoryItemId;
        entity.SerialNumber = dto.SerialNumber.Trim();
        entity.InventorySerialStatus = dto.InventorySerialStatus;
        entity.Notes = TrimOrNull(dto.Notes);

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsInventorySerialDetailDto> PatchAsync(
        long id,
        FgsInventorySerialPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory serial '{id}' was not found.");

        if (dto.InventoryItemId.HasValue)
        {
            entity.InventoryItemId = dto.InventoryItemId.Value;
        }

        if (dto.SerialNumber is not null)
        {
            entity.SerialNumber = dto.SerialNumber.Trim();
        }

        if (dto.InventorySerialStatus.HasValue)
        {
            entity.InventorySerialStatus = dto.InventorySerialStatus.Value;
        }

        if (dto.Notes is not null)
        {
            entity.Notes = TrimOrNull(dto.Notes);
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    private async Task<FgsInventorySerial?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsInventorySerials.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "A serial number with the same value already exists for the inventory item.",
                ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static FgsInventorySerialDetailDto MapToDetail(FgsInventorySerial entity) =>
        new(
            entity.Id,
            entity.InventoryItemId,
            entity.SerialNumber,
            entity.InventorySerialStatus,
            entity.Notes,
            entity.CreatedOn,
            entity.CreatedBy,
            entity.UpdatedOn,
            entity.UpdatedBy);
}
