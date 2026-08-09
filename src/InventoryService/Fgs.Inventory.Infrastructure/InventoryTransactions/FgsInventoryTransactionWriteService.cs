using Fgs.Inventory.Application.Abstractions.InventoryTransactions;
using Fgs.Foundation.Time;
using Fgs.Inventory.Application.Features.InventoryTransactions.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.InventoryTransactions;

public sealed class FgsInventoryTransactionWriteService : IFgsInventoryTransactionWriteService
{
    private readonly FgsInventoryDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryEntityAuditHelper _auditHelper;
    private readonly IDateTimeProvider _dateTimeProvider;

    public FgsInventoryTransactionWriteService(
        FgsInventoryDbContext context,
        IUnitOfWork unitOfWork,
        InventoryEntityAuditHelper auditHelper,
        IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<FgsInventoryTransactionDetailDto> CreateAsync(
        FgsInventoryTransactionCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsInventoryTransaction
        {
            TransactionNumber = dto.TransactionNumber.Trim(),
            InventoryItemId = dto.InventoryItemId,
            SerialNumber = TrimOrNull(dto.SerialNumber),
            TransactionType = dto.TransactionType.Trim().ToUpperInvariant(),
            Quantity = dto.Quantity,
            FromInventoryLocationId = dto.FromInventoryLocationId,
            ToInventoryLocationId = dto.ToInventoryLocationId,
            UnitCost = dto.UnitCost,
            TransactionDate = dto.TransactionDate ?? _dateTimeProvider.UtcNow,
            ReferenceType = TrimOrNull(dto.ReferenceType),
            ReferenceId = dto.ReferenceId,
            Notes = TrimOrNull(dto.Notes)
        };

        _auditHelper.StampForCreate(entity, entity);
        await _context.FgsInventoryTransactions.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A transaction with this number already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static FgsInventoryTransactionDetailDto MapToDetail(FgsInventoryTransaction entity) =>
        new(
            entity.Id,
            entity.TransactionNumber,
            entity.InventoryItemId,
            entity.SerialNumber,
            entity.TransactionType,
            entity.Quantity,
            entity.FromInventoryLocationId,
            entity.ToInventoryLocationId,
            entity.UnitCost,
            entity.TransactionDate,
            entity.ReferenceType,
            entity.ReferenceId,
            entity.Notes,
            entity.CreatedOn,
            entity.CreatedBy,
            entity.UpdatedOn,
            entity.UpdatedBy);
}
