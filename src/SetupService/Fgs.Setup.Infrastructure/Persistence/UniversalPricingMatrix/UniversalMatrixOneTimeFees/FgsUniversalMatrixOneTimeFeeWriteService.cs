using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.UniversalMatrixOneTimeFees;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixOneTimeFees;

public sealed class FgsUniversalMatrixOneTimeFeeWriteService : IFgsUniversalMatrixOneTimeFeeWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsUniversalMatrixOneTimeFeeWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsUniversalMatrixOneTimeFeeDetailDto> CreateAsync(
        FgsUniversalMatrixOneTimeFeeCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsUniversalMatrixOneTimeFee
        {
            UniversalPricingServiceId = dto.UniversalPricingServiceId,
            Name = dto.Name.Trim(),
            Amount = dto.Amount,
            DisplayOrder = dto.DisplayOrder
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsUniversalMatrixOneTimeFees.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsUniversalMatrixOneTimeFeeDetailDto> UpdateAsync(
        long id,
        FgsUniversalMatrixOneTimeFeeUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Matrix One-Time Fee '{id}' was not found.");

        entity.UniversalPricingServiceId = dto.UniversalPricingServiceId;
        entity.Name = dto.Name.Trim();
        entity.Amount = dto.Amount;
        entity.DisplayOrder = dto.DisplayOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsUniversalMatrixOneTimeFeeDetailDto> PatchAsync(
        long id,
        FgsUniversalMatrixOneTimeFeePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Matrix One-Time Fee '{id}' was not found.");

        if (dto.UniversalPricingServiceId.HasValue)
        {
            entity.UniversalPricingServiceId = dto.UniversalPricingServiceId.Value;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }
        if (dto.Amount.HasValue)
        {
            entity.Amount = dto.Amount.Value;
        }
        if (dto.DisplayOrder.HasValue)
        {
            entity.DisplayOrder = dto.DisplayOrder.Value;
        }
        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsUniversalMatrixOneTimeFeeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Matrix One-Time Fee '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsUniversalMatrixOneTimeFee?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsUniversalMatrixOneTimeFees.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A universal matrix one-time fee with the same key already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static FgsUniversalMatrixOneTimeFeeDetailDto MapToDetail(FgsUniversalMatrixOneTimeFee entity) =>
        new(
            entity.Id,
            entity.UniversalPricingServiceId,
            entity.Name,
            entity.Amount,
            entity.DisplayOrder,
            entity.IsActive);
}
