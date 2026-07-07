using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SalesActivityTypes;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Entities.SalesActivityTypes;

public sealed class FgsSalesActivityTypeWriteService : IFgsSalesActivityTypeWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsSalesActivityTypeWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsSalesActivityTypeDetailDto> CreateAsync(
        FgsSalesActivityTypeCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSalesActivityType
        {
            ActivityTypeCode = NormalizeCode(dto.ActivityTypeCode),
            ActivityTypeName = dto.ActivityTypeName.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
            DisplayOrder = dto.DisplayOrder,
            IsSystem = dto.IsSystem,
            AppliesToLead = dto.AppliesToLead,
            AppliesToOpportunity = dto.AppliesToOpportunity,
            AllowManualSelection = dto.AllowManualSelection
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSalesActivityTypes.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSalesActivityTypeDetailDto> UpdateAsync(
        long id,
        FgsSalesActivityTypeUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales Activity Type '{id}' was not found.");

        entity.ActivityTypeCode = NormalizeCode(dto.ActivityTypeCode);
        entity.ActivityTypeName = dto.ActivityTypeName.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.DisplayOrder = dto.DisplayOrder;
        entity.IsSystem = dto.IsSystem;
        entity.AppliesToLead = dto.AppliesToLead;
        entity.AppliesToOpportunity = dto.AppliesToOpportunity;
        entity.AllowManualSelection = dto.AllowManualSelection;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSalesActivityTypeDetailDto> PatchAsync(
        long id,
        FgsSalesActivityTypePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales Activity Type '{id}' was not found.");

        if (dto.ActivityTypeCode is not null)
        {
            entity.ActivityTypeCode = NormalizeCode(dto.ActivityTypeCode); ;
        }
        if (dto.ActivityTypeName is not null)
        {
            entity.ActivityTypeName = dto.ActivityTypeName.Trim(); ;
        }
        if (dto.Description is not null)
        {
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(); ;
        }
        if (dto.DisplayOrder.HasValue)
        {
            entity.DisplayOrder = dto.DisplayOrder.Value;
        }
        if (dto.IsSystem.HasValue)
        {
            entity.IsSystem = dto.IsSystem.Value;
        }
        if (dto.AppliesToLead.HasValue)
        {
            entity.AppliesToLead = dto.AppliesToLead.Value;
        }
        if (dto.AppliesToOpportunity.HasValue)
        {
            entity.AppliesToOpportunity = dto.AppliesToOpportunity.Value;
        }
        if (dto.AllowManualSelection.HasValue)
        {
            entity.AllowManualSelection = dto.AllowManualSelection.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSalesActivityTypeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales Activity Type '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSalesActivityType?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSalesActivityTypes.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A sales activity type with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSalesActivityTypeDetailDto MapToDetail(FgsSalesActivityType entity) =>
        new(
            entity.Id,
            entity.ActivityTypeCode,
            entity.ActivityTypeName,
            entity.Description,
            entity.DisplayOrder,
            entity.IsSystem,
            entity.AppliesToLead,
            entity.AppliesToOpportunity,
            entity.AllowManualSelection,
            entity.IsActive);
}
