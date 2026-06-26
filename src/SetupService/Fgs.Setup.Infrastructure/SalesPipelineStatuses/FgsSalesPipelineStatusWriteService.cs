using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.SalesPipelineStatuses;

public sealed class FgsSalesPipelineStatusWriteService : IFgsSalesPipelineStatusWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsSalesPipelineStatusWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsSalesPipelineStatusDetailDto> CreateAsync(
        FgsSalesPipelineStatusCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSalesPipelineStatus
        {
            StatusCode = NormalizeCode(dto.StatusCode), StatusName = dto.StatusName.Trim(), Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(), DisplayOrder = dto.DisplayOrder, IsSystem = dto.IsSystem, AppliesToLead = dto.AppliesToLead, AppliesToOpportunity = dto.AppliesToOpportunity, IsTerminal = dto.IsTerminal, AllowManualSelection = dto.AllowManualSelection
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSalesPipelineStatuses.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSalesPipelineStatusDetailDto> UpdateAsync(
        long id,
        FgsSalesPipelineStatusUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales Pipeline Status '{id}' was not found.");

        entity.StatusCode = NormalizeCode(dto.StatusCode);
        entity.StatusName = dto.StatusName.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.DisplayOrder = dto.DisplayOrder;
        entity.IsSystem = dto.IsSystem;
        entity.AppliesToLead = dto.AppliesToLead;
        entity.AppliesToOpportunity = dto.AppliesToOpportunity;
        entity.IsTerminal = dto.IsTerminal;
        entity.AllowManualSelection = dto.AllowManualSelection;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSalesPipelineStatusDetailDto> PatchAsync(
        long id,
        FgsSalesPipelineStatusPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales Pipeline Status '{id}' was not found.");

        if (dto.StatusCode is not null)
        {
            entity.StatusCode = NormalizeCode(dto.StatusCode);;
        }
        if (dto.StatusName is not null)
        {
            entity.StatusName = dto.StatusName.Trim();;
        }
        if (dto.Description is not null)
        {
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();;
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
        if (dto.IsTerminal.HasValue)
        {
            entity.IsTerminal = dto.IsTerminal.Value;
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

    public async Task<FgsSalesPipelineStatusDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales Pipeline Status '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSalesPipelineStatus?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSalesPipelineStatuses.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A sales pipeline status with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSalesPipelineStatusDetailDto MapToDetail(FgsSalesPipelineStatus entity) =>
        new(
            entity.Id,
            entity.StatusCode,
            entity.StatusName,
            entity.Description,
            entity.DisplayOrder,
            entity.IsSystem,
            entity.AppliesToLead,
            entity.AppliesToOpportunity,
            entity.IsTerminal,
            entity.AllowManualSelection,
            entity.IsActive);
}
