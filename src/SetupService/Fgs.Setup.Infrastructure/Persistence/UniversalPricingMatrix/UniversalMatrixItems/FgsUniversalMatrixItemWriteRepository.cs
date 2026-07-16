using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.UniversalMatrixItems;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixItems;

public sealed class FgsUniversalMatrixItemWriteRepository : IFgsUniversalMatrixItemWriteRepository
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsUniversalMatrixItemWriteRepository(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsUniversalMatrixItemDetailDto> CreateAsync(
        FgsUniversalMatrixItemCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsUniversalMatrixItem
        {
            UniversalPricingServiceId = dto.UniversalPricingServiceId, ItemName = dto.ItemName.Trim(), UnitType = dto.UnitType.Trim(), BasePrice = dto.BasePrice, DisplayOrder = dto.DisplayOrder
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsUniversalMatrixItems.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsUniversalMatrixItemDetailDto> UpdateAsync(
        long id,
        FgsUniversalMatrixItemUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Matrix Item '{id}' was not found.");

        entity.UniversalPricingServiceId = dto.UniversalPricingServiceId;
        entity.ItemName = dto.ItemName.Trim();
        entity.UnitType = dto.UnitType.Trim();
        entity.BasePrice = dto.BasePrice;
        entity.DisplayOrder = dto.DisplayOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsUniversalMatrixItemDetailDto> PatchAsync(
        long id,
        FgsUniversalMatrixItemPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Matrix Item '{id}' was not found.");

        if (dto.UniversalPricingServiceId.HasValue)
        {
            entity.UniversalPricingServiceId = dto.UniversalPricingServiceId.Value;
        }
        if (dto.ItemName is not null)
        {
            entity.ItemName = dto.ItemName.Trim();;
        }
        if (dto.UnitType is not null)
        {
            entity.UnitType = dto.UnitType.Trim();;
        }
        if (dto.BasePrice.HasValue)
        {
            entity.BasePrice = dto.BasePrice.Value;
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

    public async Task<FgsUniversalMatrixItemDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Matrix Item '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsUniversalMatrixItem?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsUniversalMatrixItems.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A universal matrix item with the same type and name already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsUniversalMatrixItemDetailDto MapToDetail(FgsUniversalMatrixItem entity) =>
        new(
            entity.Id,
            entity.UniversalPricingServiceId,
            entity.ItemName,
            entity.UnitType,
            entity.BasePrice,
            entity.DisplayOrder,
            entity.IsActive);
}
