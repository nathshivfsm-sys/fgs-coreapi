using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.UniversalMatrixFrequencyDiscounts;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixFrequencyDiscounts;

public sealed class FgsUniversalMatrixFrequencyDiscountWriteService : IFgsUniversalMatrixFrequencyDiscountWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsUniversalMatrixFrequencyDiscountWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsUniversalMatrixFrequencyDiscountDetailDto> CreateAsync(
        FgsUniversalMatrixFrequencyDiscountCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsUniversalMatrixFrequencyDiscount
        {
            UniversalPricingServiceId = dto.UniversalPricingServiceId,
            Name = dto.Name.Trim(),
            DiscountPercent = dto.DiscountPercent,
            DisplayOrder = dto.DisplayOrder
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsUniversalMatrixFrequencyDiscounts.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsUniversalMatrixFrequencyDiscountDetailDto> UpdateAsync(
        long id,
        FgsUniversalMatrixFrequencyDiscountUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Matrix Frequency Discount '{id}' was not found.");

        entity.UniversalPricingServiceId = dto.UniversalPricingServiceId;
        entity.Name = dto.Name.Trim();
        entity.DiscountPercent = dto.DiscountPercent;
        entity.DisplayOrder = dto.DisplayOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsUniversalMatrixFrequencyDiscountDetailDto> PatchAsync(
        long id,
        FgsUniversalMatrixFrequencyDiscountPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Matrix Frequency Discount '{id}' was not found.");

        if (dto.UniversalPricingServiceId.HasValue)
        {
            entity.UniversalPricingServiceId = dto.UniversalPricingServiceId.Value;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }
        if (dto.DiscountPercent.HasValue)
        {
            entity.DiscountPercent = dto.DiscountPercent.Value;
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

    public async Task<FgsUniversalMatrixFrequencyDiscountDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Matrix Frequency Discount '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsUniversalMatrixFrequencyDiscount?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsUniversalMatrixFrequencyDiscounts.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A universal matrix frequency discount with the same key already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static FgsUniversalMatrixFrequencyDiscountDetailDto MapToDetail(FgsUniversalMatrixFrequencyDiscount entity) =>
        new(
            entity.Id,
            entity.UniversalPricingServiceId,
            entity.Name,
            entity.DiscountPercent,
            entity.DisplayOrder,
            entity.IsActive);
}
