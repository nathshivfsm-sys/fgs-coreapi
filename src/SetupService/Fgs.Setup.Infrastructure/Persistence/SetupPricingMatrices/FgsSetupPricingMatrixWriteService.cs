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
    private readonly IFgsSetupPricingMatrixReadRepository _readRepository;

    public FgsSetupPricingMatrixWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper,
        IFgsSetupPricingMatrixReadRepository readRepository)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
        _readRepository = readRepository;
    }

    public async Task<FgsSetupPricingMatrixDetailDto> CreateAsync(
        FgsSetupPricingMatrixCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = MapHeader(new FgsSetupPricingMatrixWriteDto(
            dto.Name,
            dto.Description,
            dto.IsDefault,
            dto.IsLaborTierStructure,
            dto.IsLaborRateBySkillLevel,
            dto.PriceAdjustmentTypeId,
            dto.EffectiveFrom,
            dto.EffectiveTo,
            dto.IsMobileVisible,
            dto.LaborLines,
            dto.MaterialTiers,
            dto.OtherItems));

        _auditHelper.StampForCreate(entity);
        await _context.FgsSetupPricingMatrices.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await ApplyDefaultMatrixAsync(entity, cancellationToken);
        await SyncChildrenAsync(entity, dto.LaborLines, dto.MaterialTiers, dto.OtherItems, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return (await _readRepository.GetByIdAsync(entity.Id, cancellationToken))!;
    }

    public async Task<FgsSetupPricingMatrixDetailDto> UpdateAsync(
        long id,
        FgsSetupPricingMatrixUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindMatrixAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Pricing matrix '{id}' was not found.");

        ApplyHeader(entity, new FgsSetupPricingMatrixWriteDto(
            dto.Name,
            dto.Description,
            dto.IsDefault,
            dto.IsLaborTierStructure,
            dto.IsLaborRateBySkillLevel,
            dto.PriceAdjustmentTypeId,
            dto.EffectiveFrom,
            dto.EffectiveTo,
            dto.IsMobileVisible,
            dto.LaborLines,
            dto.MaterialTiers,
            dto.OtherItems));

        _auditHelper.StampForUpdate(entity);
        await ApplyDefaultMatrixAsync(entity, cancellationToken);
        await SyncChildrenAsync(entity, dto.LaborLines, dto.MaterialTiers, dto.OtherItems, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return (await _readRepository.GetByIdAsync(entity.Id, cancellationToken))!;
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

        return (await _readRepository.GetByIdAsync(entity.Id, cancellationToken))!;
    }

    private async Task<FgsSetupPricingMatrix?> FindMatrixAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupPricingMatrices.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private static FgsSetupPricingMatrix MapHeader(FgsSetupPricingMatrixWriteDto dto)
    {
        var hasMarkup = HasMaterialTiers(dto.MaterialTiers) || HasOtherItems(dto.OtherItems);
        return new FgsSetupPricingMatrix
        {
            Code = NormalizeCode(dto.Name),
            Name = dto.Description.Trim(),
            IsDefault = dto.IsDefault,
            IsLaborTierStructure = dto.IsLaborTierStructure,
            IsLaborRateBySkillLevel = dto.IsLaborRateBySkillLevel,
            PriceAdjustmentTypeId = ResolvePriceAdjustmentType(dto.PriceAdjustmentTypeId, hasMarkup),
            EffectiveFrom = dto.EffectiveFrom ?? DateOnly.FromDateTime(DateTime.UtcNow),
            EffectiveTo = dto.EffectiveTo,
            IsMobileVisible = dto.IsMobileVisible ?? true
        };
    }

    private static void ApplyHeader(FgsSetupPricingMatrix entity, FgsSetupPricingMatrixWriteDto dto)
    {
        var hasMarkup = HasMaterialTiers(dto.MaterialTiers) || HasOtherItems(dto.OtherItems);
        entity.Code = NormalizeCode(dto.Name);
        entity.Name = dto.Description.Trim();
        entity.IsDefault = dto.IsDefault;
        entity.IsLaborTierStructure = dto.IsLaborTierStructure;
        entity.IsLaborRateBySkillLevel = dto.IsLaborRateBySkillLevel;
        entity.PriceAdjustmentTypeId = ResolvePriceAdjustmentType(dto.PriceAdjustmentTypeId, hasMarkup);

        if (dto.EffectiveFrom.HasValue)
        {
            entity.EffectiveFrom = dto.EffectiveFrom.Value;
        }

        entity.EffectiveTo = dto.EffectiveTo;

        if (dto.IsMobileVisible.HasValue)
        {
            entity.IsMobileVisible = dto.IsMobileVisible.Value;
        }
    }

    private static PriceAdjustmentType ResolvePriceAdjustmentType(short? priceAdjustmentTypeId, bool hasMarkup) =>
        priceAdjustmentTypeId is >= 1 and <= 3
            ? (PriceAdjustmentType)priceAdjustmentTypeId.Value
            : hasMarkup
                ? throw new InvalidOperationException("PriceAdjustmentTypeId is required when markup items are provided.")
                : PriceAdjustmentType.MarkupPercent;

    private async Task ApplyDefaultMatrixAsync(FgsSetupPricingMatrix entity, CancellationToken cancellationToken)
    {
        if (!entity.IsDefault)
        {
            return;
        }

        await _context.FgsSetupPricingMatrices
            .Where(m => m.Id != entity.Id && m.IsDefault && m.IsActive)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(m => m.IsDefault, false),
                cancellationToken);
    }

    private async Task SyncChildrenAsync(
        FgsSetupPricingMatrix matrix,
        IReadOnlyList<FgsSetupPricingMatrixLaborLineDto>? laborLines,
        IReadOnlyList<FgsSetupPricingMatrixMaterialTierDto>? materialTiers,
        IReadOnlyList<FgsSetupPricingMatrixOtherItemDto>? otherItems,
        CancellationToken cancellationToken)
    {
        await SyncLaborAsync(matrix, laborLines ?? [], cancellationToken);

        if (HasMaterialTiers(materialTiers))
        {
            await SyncMaterialTiersAsync(matrix, materialTiers!, cancellationToken);
            await DeactivateOtherItemsAsync(matrix.Id, excludedIds: null, cancellationToken);
        }
        else if (HasOtherItems(otherItems))
        {
            await SyncOtherItemsAsync(matrix, otherItems!, cancellationToken);
            await DeactivateMaterialTiersAsync(matrix.Id, excludedIds: null, cancellationToken);
        }
        else
        {
            await DeactivateMaterialTiersAsync(matrix.Id, excludedIds: null, cancellationToken);
            await DeactivateOtherItemsAsync(matrix.Id, excludedIds: null, cancellationToken);
        }
    }

    private async Task SyncLaborAsync(
        FgsSetupPricingMatrix matrix,
        IReadOnlyList<FgsSetupPricingMatrixLaborLineDto> laborLines,
        CancellationToken cancellationToken)
    {
        var existingLabor = await _context.FgsSetupPricingMatrixLabors
            .Where(l => l.PricingMatrixId == matrix.Id && l.IsActive)
            .ToListAsync(cancellationToken);

        var keptLaborIds = new HashSet<long>();

        foreach (var line in laborLines)
        {
            var techSkillLevelId = matrix.IsLaborRateBySkillLevel ? line.TechSkillLevelId : null;
            var labor = line.Id.HasValue
                ? existingLabor.FirstOrDefault(l => l.Id == line.Id.Value)
                : null;

            if (labor is null)
            {
                labor = new FgsSetupPricingMatrixLabor
                {
                    PricingMatrixId = matrix.Id
                };
                _auditHelper.StampForCreate(labor);
                await _context.FgsSetupPricingMatrixLabors.AddAsync(labor, cancellationToken);
            }
            else
            {
                _auditHelper.StampForUpdate(labor);
            }

            labor.LaborRateTypeId = line.LaborRateTypeId;
            labor.TechSkillLevelId = techSkillLevelId;
            labor.IsActive = true;

            if (matrix.IsLaborTierStructure)
            {
                labor.BaseRate = 0;
                labor.OvertimeMultiplier = null;
                labor.DoubleTimeMultiplier = null;
                labor.DiscountPercent = null;
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await SyncLaborTiersAsync(matrix, labor, line.Tiers ?? [], cancellationToken);
            }
            else
            {
                labor.BaseRate = line.BaseRate ?? 0;
                labor.OvertimeMultiplier = line.OvertimeMultiplier;
                labor.DoubleTimeMultiplier = line.DoubleTimeMultiplier;
                labor.DiscountPercent = line.DiscountPercent;
                await DeactivateLaborTiersForLaborAsync(labor.Id, excludedIds: null, cancellationToken);
            }

            keptLaborIds.Add(labor.Id);
        }

        foreach (var labor in existingLabor.Where(l => !keptLaborIds.Contains(l.Id)))
        {
            labor.IsActive = false;
            _auditHelper.StampForUpdate(labor);
            await DeactivateLaborTiersForLaborAsync(labor.Id, excludedIds: null, cancellationToken);
        }

        if (!matrix.IsLaborTierStructure)
        {
            await DeactivateAllLaborTiersForMatrixAsync(matrix.Id, cancellationToken);
        }
    }

    private async Task SyncLaborTiersAsync(
        FgsSetupPricingMatrix matrix,
        FgsSetupPricingMatrixLabor labor,
        IReadOnlyList<FgsSetupPricingMatrixLaborTierItemDto> tiers,
        CancellationToken cancellationToken)
    {
        var existingTiers = await _context.FgsSetupPricingMatrixLaborTiers
            .Where(t => t.PricingMatrixLaborId == labor.Id && t.IsActive)
            .ToListAsync(cancellationToken);

        var keptTierIds = new HashSet<long>();

        foreach (var tierDto in tiers)
        {
            var tier = tierDto.Id.HasValue
                ? existingTiers.FirstOrDefault(t => t.Id == tierDto.Id.Value)
                : null;

            if (tier is null)
            {
                tier = new FgsSetupPricingMatrixLaborTier
                {
                    PricingMatrixLaborId = labor.Id,
                    SequenceOrder = tierDto.SequenceOrder,
                    DurationMinutes = tierDto.DurationMinutes,
                    Rate = tierDto.Rate,
                    TechSkillLevelId = matrix.IsLaborRateBySkillLevel ? tierDto.TechSkillLevelId : null
                };
                _auditHelper.StampForCreate(tier);
                await _context.FgsSetupPricingMatrixLaborTiers.AddAsync(tier, cancellationToken);
            }
            else
            {
                _auditHelper.StampForUpdate(tier);
                tier.SequenceOrder = tierDto.SequenceOrder;
                tier.DurationMinutes = tierDto.DurationMinutes;
                tier.Rate = tierDto.Rate;
                tier.TechSkillLevelId = matrix.IsLaborRateBySkillLevel ? tierDto.TechSkillLevelId : null;
            }

            tier.IsActive = true;
            if (tier.Id != 0)
            {
                keptTierIds.Add(tier.Id);
            }
        }

        foreach (var tier in existingTiers.Where(t => !keptTierIds.Contains(t.Id)))
        {
            tier.IsActive = false;
            _auditHelper.StampForUpdate(tier);
        }
    }

    private async Task SyncMaterialTiersAsync(
        FgsSetupPricingMatrix matrix,
        IReadOnlyList<FgsSetupPricingMatrixMaterialTierDto> materialTiers,
        CancellationToken cancellationToken)
    {
        var existing = await _context.FgsSetupPricingMatrixMaterialTiers
            .Where(m => m.PricingMatrixId == matrix.Id && m.IsActive)
            .ToListAsync(cancellationToken);

        var keptIds = new HashSet<long>();

        foreach (var dto in materialTiers)
        {
            var tier = dto.Id.HasValue
                ? existing.FirstOrDefault(m => m.Id == dto.Id.Value)
                : null;

            if (tier is null)
            {
                tier = new FgsSetupPricingMatrixMaterialTier
                {
                    PricingMatrixId = matrix.Id
                };
                _auditHelper.StampForCreate(tier);
                await _context.FgsSetupPricingMatrixMaterialTiers.AddAsync(tier, cancellationToken);
            }
            else
            {
                _auditHelper.StampForUpdate(tier);
            }

            tier.FromCost = dto.FromCost;
            tier.ToCost = dto.ToCost;
            tier.AdjustmentValue = dto.AdjustmentValue;
            tier.IsActive = true;
            keptIds.Add(tier.Id);
        }

        await DeactivateMaterialTiersAsync(matrix.Id, keptIds, cancellationToken);
    }

    private async Task SyncOtherItemsAsync(
        FgsSetupPricingMatrix matrix,
        IReadOnlyList<FgsSetupPricingMatrixOtherItemDto> otherItems,
        CancellationToken cancellationToken)
    {
        var existing = await _context.FgsSetupPricingMatrixOthers
            .Where(o => o.PricingMatrixId == matrix.Id && o.IsActive)
            .ToListAsync(cancellationToken);

        var keptIds = new HashSet<long>();

        foreach (var dto in otherItems)
        {
            var item = dto.Id.HasValue
                ? existing.FirstOrDefault(o => o.Id == dto.Id.Value)
                : null;

            if (item is null)
            {
                item = new FgsSetupPricingMatrixOther
                {
                    PricingMatrixId = matrix.Id,
                    CategoryCode = NormalizeCategoryCode(dto.CategoryCode)
                };
                _auditHelper.StampForCreate(item);
                await _context.FgsSetupPricingMatrixOthers.AddAsync(item, cancellationToken);
            }
            else
            {
                _auditHelper.StampForUpdate(item);
            }

            item.CategoryCode = NormalizeCategoryCode(dto.CategoryCode);
            item.Name = dto.Name.Trim();
            item.AdjustmentValue = dto.AdjustmentValue;
            item.DiscountPercent = dto.DiscountPercent;
            item.IsActive = true;
            keptIds.Add(item.Id);
        }

        await DeactivateOtherItemsAsync(matrix.Id, keptIds, cancellationToken);
    }

    private async Task DeactivateMaterialTiersAsync(
        long pricingMatrixId,
        HashSet<long>? excludedIds,
        CancellationToken cancellationToken)
    {
        var tiers = await _context.FgsSetupPricingMatrixMaterialTiers
            .Where(m => m.PricingMatrixId == pricingMatrixId && m.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var tier in tiers.Where(t => excludedIds is null || !excludedIds.Contains(t.Id)))
        {
            tier.IsActive = false;
            _auditHelper.StampForUpdate(tier);
        }
    }

    private async Task DeactivateOtherItemsAsync(
        long pricingMatrixId,
        HashSet<long>? excludedIds,
        CancellationToken cancellationToken)
    {
        var items = await _context.FgsSetupPricingMatrixOthers
            .Where(o => o.PricingMatrixId == pricingMatrixId && o.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var item in items.Where(o => excludedIds is null || !excludedIds.Contains(o.Id)))
        {
            item.IsActive = false;
            _auditHelper.StampForUpdate(item);
        }
    }

    private async Task DeactivateLaborTiersForLaborAsync(
        long pricingMatrixLaborId,
        HashSet<long>? excludedIds,
        CancellationToken cancellationToken)
    {
        var tiers = await _context.FgsSetupPricingMatrixLaborTiers
            .Where(t => t.PricingMatrixLaborId == pricingMatrixLaborId && t.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var tier in tiers.Where(t => excludedIds is null || !excludedIds.Contains(t.Id)))
        {
            tier.IsActive = false;
            _auditHelper.StampForUpdate(tier);
        }
    }

    private async Task DeactivateAllLaborTiersForMatrixAsync(long pricingMatrixId, CancellationToken cancellationToken)
    {
        var laborIds = await _context.FgsSetupPricingMatrixLabors
            .Where(l => l.PricingMatrixId == pricingMatrixId)
            .Select(l => l.Id)
            .ToListAsync(cancellationToken);

        foreach (var laborId in laborIds)
        {
            await DeactivateLaborTiersForLaborAsync(laborId, excludedIds: null, cancellationToken);
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

    private static bool HasMaterialTiers(IReadOnlyList<FgsSetupPricingMatrixMaterialTierDto>? materialTiers) =>
        materialTiers is { Count: > 0 };

    private static bool HasOtherItems(IReadOnlyList<FgsSetupPricingMatrixOtherItemDto>? otherItems) =>
        otherItems is { Count: > 0 };

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static string NormalizeCategoryCode(string code) => code.Trim().ToUpperInvariant();
}
