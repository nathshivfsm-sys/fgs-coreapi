using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Domain.Enums;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrices;

public sealed class FgsSetupPricingMatrixWriteService : IFgsSetupPricingMatrixWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsSetupPricingMatrixWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsSetupPricingMatrixDetailDto> CreateAsync(
        FgsSetupPricingMatrixCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupPricingMatrix
        {
            Code = NormalizeCode(dto.Name),
            Name = dto.Description.Trim(),
            IsDefault = dto.IsDefault,
            IsLaborTierStructure = dto.IsLaborTierStructure,
            IsLaborRateBySkillLevel = dto.IsLaborRateBySkillLevel,
            PriceAdjustmentTypeId = ResolvePriceAdjustmentType(dto.PriceAdjustmentTypeId),
            EffectiveFrom = dto.EffectiveFrom ?? DateOnly.FromDateTime(DateTime.UtcNow),
            EffectiveTo = dto.EffectiveTo,
            IsMobileVisible = dto.IsMobileVisible ?? true
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSetupPricingMatrices.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await ApplyDefaultMatrixAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupPricingMatrixDetailDto> UpdateAsync(
        long id,
        FgsSetupPricingMatrixUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindMatrixAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Pricing matrix '{id}' was not found.");

        entity.Code = NormalizeCode(dto.Name);
        entity.Name = dto.Description.Trim();
        entity.IsDefault = dto.IsDefault;
        entity.IsLaborTierStructure = dto.IsLaborTierStructure;
        entity.IsLaborRateBySkillLevel = dto.IsLaborRateBySkillLevel;
        entity.PriceAdjustmentTypeId = ResolvePriceAdjustmentType(dto.PriceAdjustmentTypeId);

        if (dto.EffectiveFrom.HasValue)
        {
            entity.EffectiveFrom = dto.EffectiveFrom.Value;
        }

        entity.EffectiveTo = dto.EffectiveTo;

        if (dto.IsMobileVisible.HasValue)
        {
            entity.IsMobileVisible = dto.IsMobileVisible.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await ApplyDefaultMatrixAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupPricingMatrixDetailDto> PatchAsync(
        long id,
        FgsSetupPricingMatrixPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindMatrixAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Pricing matrix '{id}' was not found.");

        if (dto.Name is not null)
        {
            entity.Code = NormalizeCode(dto.Name);
        }

        if (dto.Description is not null)
        {
            entity.Name = dto.Description.Trim();
        }

        if (dto.IsDefault.HasValue)
        {
            entity.IsDefault = dto.IsDefault.Value;
        }

        if (dto.IsLaborTierStructure.HasValue)
        {
            entity.IsLaborTierStructure = dto.IsLaborTierStructure.Value;
        }

        if (dto.IsLaborRateBySkillLevel.HasValue)
        {
            entity.IsLaborRateBySkillLevel = dto.IsLaborRateBySkillLevel.Value;
        }

        if (dto.PriceAdjustmentTypeId.HasValue)
        {
            entity.PriceAdjustmentTypeId = (PriceAdjustmentType)dto.PriceAdjustmentTypeId.Value;
        }

        if (dto.EffectiveFrom.HasValue)
        {
            entity.EffectiveFrom = dto.EffectiveFrom.Value;
        }

        if (dto.EffectiveTo.HasValue)
        {
            entity.EffectiveTo = dto.EffectiveTo;
        }

        if (dto.IsMobileVisible.HasValue)
        {
            entity.IsMobileVisible = dto.IsMobileVisible.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await ApplyDefaultMatrixAsync(entity, cancellationToken);

        if (dto.IsLaborRateBySkillLevel == false)
        {
            await ClearSkillLevelsAsync(entity.Id, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    private async Task<FgsSetupPricingMatrix?> FindMatrixAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupPricingMatrices.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private static PriceAdjustmentType ResolvePriceAdjustmentType(short? priceAdjustmentTypeId) =>
        priceAdjustmentTypeId is >= 1 and <= 3
            ? (PriceAdjustmentType)priceAdjustmentTypeId.Value
            : PriceAdjustmentType.MarkupPercent;

    private async Task ApplyDefaultMatrixAsync(FgsSetupPricingMatrix entity, CancellationToken cancellationToken)
    {
        if (!entity.IsDefault)
        {
            return;
        }

        var previousDefaults = await _context.FgsSetupPricingMatrices
            .Where(m => m.Id != entity.Id && m.IsDefault && m.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var previousDefault in previousDefaults)
        {
            previousDefault.IsDefault = false;
            _auditHelper.StampForUpdate(previousDefault);
        }
    }

    private async Task ClearSkillLevelsAsync(long pricingMatrixId, CancellationToken cancellationToken)
    {
        var labors = await _context.FgsSetupPricingMatrixLabors
            .Where(l => l.PricingMatrixId == pricingMatrixId && l.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var labor in labors)
        {
            labor.TechSkillLevelId = null;
            _auditHelper.StampForUpdate(labor);
        }

        var laborIds = labors.Select(l => l.Id).ToList();
        var tiers = await _context.FgsSetupPricingMatrixLaborTiers
            .Where(t => laborIds.Contains(t.PricingMatrixLaborId) && t.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var tier in tiers)
        {
            tier.TechSkillLevelId = null;
            _auditHelper.StampForUpdate(tier);
        }
    }

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSetupPricingMatrixDetailDto MapToDetail(FgsSetupPricingMatrix entity) =>
        new(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsDefault,
            entity.IsLaborTierStructure,
            entity.IsLaborRateBySkillLevel,
            (short)entity.PriceAdjustmentTypeId,
            entity.EffectiveFrom,
            entity.EffectiveTo,
            entity.IsMobileVisible,
            entity.IsActive);
}
