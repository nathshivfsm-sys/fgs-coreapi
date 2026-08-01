using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.UniversalPricingServices;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalPricingServices;

public sealed class FgsUniversalPricingServiceWriteService : IFgsUniversalPricingServiceWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsUniversalPricingServiceWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsUniversalPricingServiceDetailDto> CreateAsync(
        FgsUniversalPricingServiceCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsUniversalPricingService
        {
            UniversalPricingServiceCode = NormalizeCode(dto.UniversalPricingServiceCode),
            DisplayOrder = dto.DisplayOrder
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsUniversalPricingServices.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        await SyncChildrenAsync(
            entity,
            dto.Tiers,
            dto.SizeTiers,
            dto.Items,
            dto.FrequencyDiscounts,
            dto.OneTimeFees,
            dto.AddOns,
            cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return await MapDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<FgsUniversalPricingServiceDetailDto> UpdateAsync(
        long id,
        FgsUniversalPricingServiceUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Pricing Service '{id}' was not found.");

        entity.UniversalPricingServiceCode = NormalizeCode(dto.UniversalPricingServiceCode);
        entity.DisplayOrder = dto.DisplayOrder;
        _auditHelper.StampForUpdate(entity);

        await SyncChildrenAsync(
            entity,
            dto.Tiers,
            dto.SizeTiers,
            dto.Items,
            dto.FrequencyDiscounts,
            dto.OneTimeFees,
            dto.AddOns,
            cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return await MapDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<FgsUniversalPricingServiceDetailDto> PatchAsync(
        long id,
        FgsUniversalPricingServicePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Pricing Service '{id}' was not found.");

        if (dto.UniversalPricingServiceCode is not null)
        {
            entity.UniversalPricingServiceCode = NormalizeCode(dto.UniversalPricingServiceCode);
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

        return await MapDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<FgsUniversalPricingServiceDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Pricing Service '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return await MapDetailAsync(entity.Id, cancellationToken);
    }

    private async Task<FgsUniversalPricingServiceDetailDto> MapDetailAsync(long id, CancellationToken cancellationToken)
    {
        var header = await _context.FgsUniversalPricingServices
            .AsNoTracking()
            .FirstAsync(e => e.Id == id, cancellationToken);

        var tiers = await _context.FgsUniversalMatrixTiers.AsNoTracking()
            .Where(e => e.UniversalPricingServiceId == id && e.IsActive)
            .OrderBy(e => e.DisplayOrder).ThenBy(e => e.Name)
            .Select(e => new FgsUniversalMatrixTierDetailDto(e.Id, e.Name, e.Multiplier, e.DisplayOrder, e.IsActive))
            .ToListAsync(cancellationToken);

        var sizeTiers = await _context.FgsUniversalMatrixSizeTiers.AsNoTracking()
            .Where(e => e.UniversalPricingServiceId == id && e.IsActive)
            .OrderBy(e => e.DisplayOrder).ThenBy(e => e.Name)
            .Select(e => new FgsUniversalMatrixSizeTierDetailDto(e.Id, e.Name, e.Multiplier, e.DisplayOrder, e.IsActive))
            .ToListAsync(cancellationToken);

        var items = await _context.FgsUniversalMatrixItems.AsNoTracking()
            .Where(e => e.UniversalPricingServiceId == id && e.IsActive)
            .OrderBy(e => e.DisplayOrder).ThenBy(e => e.ItemName)
            .Select(e => new FgsUniversalMatrixItemDetailDto(e.Id, e.ItemName, e.UnitType, e.BasePrice, e.DisplayOrder, e.IsActive))
            .ToListAsync(cancellationToken);

        var frequencyDiscounts = await _context.FgsUniversalMatrixFrequencyDiscounts.AsNoTracking()
            .Where(e => e.UniversalPricingServiceId == id && e.IsActive)
            .OrderBy(e => e.DisplayOrder).ThenBy(e => e.Name)
            .Select(e => new FgsUniversalMatrixFrequencyDiscountDetailDto(e.Id, e.Name, e.DiscountPercent, e.DisplayOrder, e.IsActive))
            .ToListAsync(cancellationToken);

        var oneTimeFees = await _context.FgsUniversalMatrixOneTimeFees.AsNoTracking()
            .Where(e => e.UniversalPricingServiceId == id && e.IsActive)
            .OrderBy(e => e.DisplayOrder).ThenBy(e => e.Name)
            .Select(e => new FgsUniversalMatrixOneTimeFeeDetailDto(e.Id, e.Name, e.Amount, e.DisplayOrder, e.IsActive))
            .ToListAsync(cancellationToken);

        var addOns = await _context.FgsUniversalMatrixAddOns.AsNoTracking()
            .Where(e => e.UniversalPricingServiceId == id && e.IsActive)
            .OrderBy(e => e.DisplayOrder).ThenBy(e => e.Name)
            .Select(e => new FgsUniversalMatrixAddOnDetailDto(e.Id, e.Name, e.UnitType, e.Price, e.DisplayOrder, e.IsActive))
            .ToListAsync(cancellationToken);

        return new FgsUniversalPricingServiceDetailDto(
            header.Id,
            header.UniversalPricingServiceCode,
            header.DisplayOrder,
            header.IsActive,
            tiers,
            sizeTiers,
            items,
            frequencyDiscounts,
            oneTimeFees,
            addOns);
    }

    private async Task SyncChildrenAsync(
        FgsUniversalPricingService parent,
        IReadOnlyList<FgsUniversalMatrixTierItemDto>? tiers,
        IReadOnlyList<FgsUniversalMatrixSizeTierItemDto>? sizeTiers,
        IReadOnlyList<FgsUniversalMatrixItemItemDto>? items,
        IReadOnlyList<FgsUniversalMatrixFrequencyDiscountItemDto>? frequencyDiscounts,
        IReadOnlyList<FgsUniversalMatrixOneTimeFeeItemDto>? oneTimeFees,
        IReadOnlyList<FgsUniversalMatrixAddOnItemDto>? addOns,
        CancellationToken cancellationToken)
    {
        await SyncTiersAsync(parent, tiers ?? [], cancellationToken);
        await SyncSizeTiersAsync(parent, sizeTiers ?? [], cancellationToken);
        await SyncItemsAsync(parent, items ?? [], cancellationToken);
        await SyncFrequencyDiscountsAsync(parent, frequencyDiscounts ?? [], cancellationToken);
        await SyncOneTimeFeesAsync(parent, oneTimeFees ?? [], cancellationToken);
        await SyncAddOnsAsync(parent, addOns ?? [], cancellationToken);
    }

    private async Task SyncTiersAsync(
        FgsUniversalPricingService parent,
        IReadOnlyList<FgsUniversalMatrixTierItemDto> items,
        CancellationToken cancellationToken)
    {
        var existing = await _context.FgsUniversalMatrixTiers
            .Where(e => e.UniversalPricingServiceId == parent.Id && e.IsActive)
            .ToListAsync(cancellationToken);
        var keptIds = new HashSet<long>();

        foreach (var dto in items)
        {
            var entity = dto.Id.HasValue
                ? existing.FirstOrDefault(e => e.Id == dto.Id.Value)
                : null;

            if (entity is null)
            {
                entity = new FgsUniversalMatrixTier { UniversalPricingServiceId = parent.Id };
                _auditHelper.StampForCreate(entity);
                await _context.FgsUniversalMatrixTiers.AddAsync(entity, cancellationToken);
            }
            else
            {
                _auditHelper.StampForUpdate(entity);
            }

            entity.Name = dto.Name.Trim();
            entity.Multiplier = dto.Multiplier;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.IsActive = true;
            if (entity.Id != 0)
            {
                keptIds.Add(entity.Id);
            }
        }

        foreach (var orphan in existing.Where(e => !keptIds.Contains(e.Id)))
        {
            orphan.IsActive = false;
            _auditHelper.StampForUpdate(orphan);
        }
    }

    private async Task SyncSizeTiersAsync(
        FgsUniversalPricingService parent,
        IReadOnlyList<FgsUniversalMatrixSizeTierItemDto> items,
        CancellationToken cancellationToken)
    {
        var existing = await _context.FgsUniversalMatrixSizeTiers
            .Where(e => e.UniversalPricingServiceId == parent.Id && e.IsActive)
            .ToListAsync(cancellationToken);
        var keptIds = new HashSet<long>();

        foreach (var dto in items)
        {
            var entity = dto.Id.HasValue
                ? existing.FirstOrDefault(e => e.Id == dto.Id.Value)
                : null;

            if (entity is null)
            {
                entity = new FgsUniversalMatrixSizeTier { UniversalPricingServiceId = parent.Id };
                _auditHelper.StampForCreate(entity);
                await _context.FgsUniversalMatrixSizeTiers.AddAsync(entity, cancellationToken);
            }
            else
            {
                _auditHelper.StampForUpdate(entity);
            }

            entity.Name = dto.Name.Trim();
            entity.Multiplier = dto.Multiplier;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.IsActive = true;
            if (entity.Id != 0)
            {
                keptIds.Add(entity.Id);
            }
        }

        foreach (var orphan in existing.Where(e => !keptIds.Contains(e.Id)))
        {
            orphan.IsActive = false;
            _auditHelper.StampForUpdate(orphan);
        }
    }

    private async Task SyncItemsAsync(
        FgsUniversalPricingService parent,
        IReadOnlyList<FgsUniversalMatrixItemItemDto> items,
        CancellationToken cancellationToken)
    {
        var existing = await _context.FgsUniversalMatrixItems
            .Where(e => e.UniversalPricingServiceId == parent.Id && e.IsActive)
            .ToListAsync(cancellationToken);
        var keptIds = new HashSet<long>();

        foreach (var dto in items)
        {
            var entity = dto.Id.HasValue
                ? existing.FirstOrDefault(e => e.Id == dto.Id.Value)
                : null;

            if (entity is null)
            {
                entity = new FgsUniversalMatrixItem { UniversalPricingServiceId = parent.Id };
                _auditHelper.StampForCreate(entity);
                await _context.FgsUniversalMatrixItems.AddAsync(entity, cancellationToken);
            }
            else
            {
                _auditHelper.StampForUpdate(entity);
            }

            entity.ItemName = dto.ItemName.Trim();
            entity.UnitType = dto.UnitType.Trim();
            entity.BasePrice = dto.BasePrice;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.IsActive = true;
            if (entity.Id != 0)
            {
                keptIds.Add(entity.Id);
            }
        }

        foreach (var orphan in existing.Where(e => !keptIds.Contains(e.Id)))
        {
            orphan.IsActive = false;
            _auditHelper.StampForUpdate(orphan);
        }
    }

    private async Task SyncFrequencyDiscountsAsync(
        FgsUniversalPricingService parent,
        IReadOnlyList<FgsUniversalMatrixFrequencyDiscountItemDto> items,
        CancellationToken cancellationToken)
    {
        var existing = await _context.FgsUniversalMatrixFrequencyDiscounts
            .Where(e => e.UniversalPricingServiceId == parent.Id && e.IsActive)
            .ToListAsync(cancellationToken);
        var keptIds = new HashSet<long>();

        foreach (var dto in items)
        {
            var entity = dto.Id.HasValue
                ? existing.FirstOrDefault(e => e.Id == dto.Id.Value)
                : null;

            if (entity is null)
            {
                entity = new FgsUniversalMatrixFrequencyDiscount { UniversalPricingServiceId = parent.Id };
                _auditHelper.StampForCreate(entity);
                await _context.FgsUniversalMatrixFrequencyDiscounts.AddAsync(entity, cancellationToken);
            }
            else
            {
                _auditHelper.StampForUpdate(entity);
            }

            entity.Name = dto.Name.Trim();
            entity.DiscountPercent = dto.DiscountPercent;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.IsActive = true;
            if (entity.Id != 0)
            {
                keptIds.Add(entity.Id);
            }
        }

        foreach (var orphan in existing.Where(e => !keptIds.Contains(e.Id)))
        {
            orphan.IsActive = false;
            _auditHelper.StampForUpdate(orphan);
        }
    }

    private async Task SyncOneTimeFeesAsync(
        FgsUniversalPricingService parent,
        IReadOnlyList<FgsUniversalMatrixOneTimeFeeItemDto> items,
        CancellationToken cancellationToken)
    {
        var existing = await _context.FgsUniversalMatrixOneTimeFees
            .Where(e => e.UniversalPricingServiceId == parent.Id && e.IsActive)
            .ToListAsync(cancellationToken);
        var keptIds = new HashSet<long>();

        foreach (var dto in items)
        {
            var entity = dto.Id.HasValue
                ? existing.FirstOrDefault(e => e.Id == dto.Id.Value)
                : null;

            if (entity is null)
            {
                entity = new FgsUniversalMatrixOneTimeFee { UniversalPricingServiceId = parent.Id };
                _auditHelper.StampForCreate(entity);
                await _context.FgsUniversalMatrixOneTimeFees.AddAsync(entity, cancellationToken);
            }
            else
            {
                _auditHelper.StampForUpdate(entity);
            }

            entity.Name = dto.Name.Trim();
            entity.Amount = dto.Amount;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.IsActive = true;
            if (entity.Id != 0)
            {
                keptIds.Add(entity.Id);
            }
        }

        foreach (var orphan in existing.Where(e => !keptIds.Contains(e.Id)))
        {
            orphan.IsActive = false;
            _auditHelper.StampForUpdate(orphan);
        }
    }

    private async Task SyncAddOnsAsync(
        FgsUniversalPricingService parent,
        IReadOnlyList<FgsUniversalMatrixAddOnItemDto> items,
        CancellationToken cancellationToken)
    {
        var existing = await _context.FgsUniversalMatrixAddOns
            .Where(e => e.UniversalPricingServiceId == parent.Id && e.IsActive)
            .ToListAsync(cancellationToken);
        var keptIds = new HashSet<long>();

        foreach (var dto in items)
        {
            var entity = dto.Id.HasValue
                ? existing.FirstOrDefault(e => e.Id == dto.Id.Value)
                : null;

            if (entity is null)
            {
                entity = new FgsUniversalMatrixAddOn { UniversalPricingServiceId = parent.Id };
                _auditHelper.StampForCreate(entity);
                await _context.FgsUniversalMatrixAddOns.AddAsync(entity, cancellationToken);
            }
            else
            {
                _auditHelper.StampForUpdate(entity);
            }

            entity.Name = dto.Name.Trim();
            entity.UnitType = dto.UnitType.Trim();
            entity.Price = dto.Price;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.IsActive = true;
            if (entity.Id != 0)
            {
                keptIds.Add(entity.Id);
            }
        }

        foreach (var orphan in existing.Where(e => !keptIds.Contains(e.Id)))
        {
            orphan.IsActive = false;
            _auditHelper.StampForUpdate(orphan);
        }
    }

    private async Task<FgsUniversalPricingService?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsUniversalPricingServices.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A universal pricing matrix record with the same key already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();
}
