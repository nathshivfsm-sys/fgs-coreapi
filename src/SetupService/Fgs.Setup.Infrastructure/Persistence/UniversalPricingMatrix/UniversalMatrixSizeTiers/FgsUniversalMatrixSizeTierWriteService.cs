using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.UniversalMatrixSizeTiers;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixSizeTiers;

public sealed class FgsUniversalMatrixSizeTierWriteService : IFgsUniversalMatrixSizeTierWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsUniversalMatrixSizeTierWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsUniversalMatrixSizeTierDetailDto> CreateAsync(
        FgsUniversalMatrixSizeTierCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsUniversalMatrixSizeTier
        {
            UniversalPricingServiceId = dto.UniversalPricingServiceId,
            Name = dto.Name.Trim(),
            Multiplier = dto.Multiplier,
            DisplayOrder = dto.DisplayOrder
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsUniversalMatrixSizeTiers.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsUniversalMatrixSizeTierDetailDto> UpdateAsync(
        long id,
        FgsUniversalMatrixSizeTierUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Matrix Size Tier '{id}' was not found.");

        entity.UniversalPricingServiceId = dto.UniversalPricingServiceId;
        entity.Name = dto.Name.Trim();
        entity.Multiplier = dto.Multiplier;
        entity.DisplayOrder = dto.DisplayOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsUniversalMatrixSizeTierDetailDto> PatchAsync(
        long id,
        FgsUniversalMatrixSizeTierPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Matrix Size Tier '{id}' was not found.");

        if (dto.UniversalPricingServiceId.HasValue)
        {
            entity.UniversalPricingServiceId = dto.UniversalPricingServiceId.Value;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }
        if (dto.Multiplier.HasValue)
        {
            entity.Multiplier = dto.Multiplier.Value;
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

    public async Task<FgsUniversalMatrixSizeTierDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Matrix Size Tier '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsUniversalMatrixSizeTier?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsUniversalMatrixSizeTiers.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A universal matrix size tier with the same key already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static FgsUniversalMatrixSizeTierDetailDto MapToDetail(FgsUniversalMatrixSizeTier entity) =>
        new(
            entity.Id,
            entity.UniversalPricingServiceId,
            entity.Name,
            entity.Multiplier,
            entity.DisplayOrder,
            entity.IsActive);
}
