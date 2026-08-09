using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.UniversalMatrixAddOns;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixAddOns;

public sealed class FgsUniversalMatrixAddOnWriteService : IFgsUniversalMatrixAddOnWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsUniversalMatrixAddOnWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsUniversalMatrixAddOnDetailDto> CreateAsync(
        FgsUniversalMatrixAddOnCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsUniversalMatrixAddOn
        {
            UniversalPricingServiceId = dto.UniversalPricingServiceId,
            Name = dto.Name.Trim(),
            UnitType = dto.UnitType.Trim(),
            Price = dto.Price,
            DisplayOrder = dto.DisplayOrder
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsUniversalMatrixAddOns.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsUniversalMatrixAddOnDetailDto> UpdateAsync(
        long id,
        FgsUniversalMatrixAddOnUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Matrix Add-On '{id}' was not found.");

        entity.UniversalPricingServiceId = dto.UniversalPricingServiceId;
        entity.Name = dto.Name.Trim();
        entity.UnitType = dto.UnitType.Trim();
        entity.Price = dto.Price;
        entity.DisplayOrder = dto.DisplayOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsUniversalMatrixAddOnDetailDto> PatchAsync(
        long id,
        FgsUniversalMatrixAddOnPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Matrix Add-On '{id}' was not found.");

        if (dto.UniversalPricingServiceId.HasValue)
        {
            entity.UniversalPricingServiceId = dto.UniversalPricingServiceId.Value;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }
        if (dto.UnitType is not null)
        {
            entity.UnitType = dto.UnitType.Trim();
        }
        if (dto.Price.HasValue)
        {
            entity.Price = dto.Price.Value;
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

    public async Task<FgsUniversalMatrixAddOnDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Matrix Add-On '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsUniversalMatrixAddOn?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsUniversalMatrixAddOns.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A universal matrix add-on with the same key already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static FgsUniversalMatrixAddOnDetailDto MapToDetail(FgsUniversalMatrixAddOn entity) =>
        new(
            entity.Id,
            entity.UniversalPricingServiceId,
            entity.Name,
            entity.UnitType,
            entity.Price,
            entity.DisplayOrder,
            entity.IsActive);
}
