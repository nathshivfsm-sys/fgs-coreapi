using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.BillingCategories;

public sealed class BillingCategoryWriteService : IBillingCategoryWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public BillingCategoryWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<BillingCategoryDetailDto> CreateAsync(
        BillingCategoryCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsBillingCategory
        {
            BillingCategoryType = NormalizeCode(dto.BillingCategoryType), BillingCategoryName = dto.BillingCategoryName.Trim(), Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(), DisplayOrder = dto.DisplayOrder ?? 1, IsSystemDefined = dto.IsSystemDefined, ShowToFieldTech = dto.ShowToFieldTech, AllowToPick = dto.AllowToPick
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsBillingCategories.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<BillingCategoryDetailDto> UpdateAsync(
        long id,
        BillingCategoryUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Billing Category '{id}' was not found.");

        entity.BillingCategoryType = NormalizeCode(dto.BillingCategoryType);
        entity.BillingCategoryName = dto.BillingCategoryName.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.DisplayOrder = dto.DisplayOrder ?? entity.DisplayOrder;
        entity.IsSystemDefined = dto.IsSystemDefined;
        entity.ShowToFieldTech = dto.ShowToFieldTech;
        entity.AllowToPick = dto.AllowToPick;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<BillingCategoryDetailDto> PatchAsync(
        long id,
        BillingCategoryPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Billing Category '{id}' was not found.");

        if (dto.BillingCategoryType is not null)
        {
            entity.BillingCategoryType = NormalizeCode(dto.BillingCategoryType);;
        }
        if (dto.BillingCategoryName is not null)
        {
            entity.BillingCategoryName = dto.BillingCategoryName.Trim();;
        }
        if (dto.Description is not null)
        {
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();;
        }
        if (dto.DisplayOrder.HasValue)
        {
            entity.DisplayOrder = dto.DisplayOrder.Value;
        }
        if (dto.IsSystemDefined.HasValue)
        {
            entity.IsSystemDefined = dto.IsSystemDefined.Value;
        }
        if (dto.ShowToFieldTech.HasValue)
        {
            entity.ShowToFieldTech = dto.ShowToFieldTech.Value;
        }
        if (dto.AllowToPick.HasValue)
        {
            entity.AllowToPick = dto.AllowToPick.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<BillingCategoryDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Billing Category '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsBillingCategory?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsBillingCategories.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A billing category with the same type and name already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static BillingCategoryDetailDto MapToDetail(FgsBillingCategory entity) =>
        new(
            entity.Id,
            entity.TenantId,
            entity.CompanyId,
            entity.BillingCategoryType,
            entity.BillingCategoryName,
            entity.Description,
            entity.DisplayOrder,
            entity.IsSystemDefined,
            entity.ShowToFieldTech,
            entity.AllowToPick,
            entity.IsActive,
            entity.CreatedOn,
            entity.CreatedBy,
            entity.UpdatedOn,
            entity.UpdatedBy);
}
