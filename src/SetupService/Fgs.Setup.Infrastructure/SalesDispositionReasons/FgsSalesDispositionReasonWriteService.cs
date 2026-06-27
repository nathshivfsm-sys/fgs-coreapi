using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.SalesDispositionReasons;

public sealed class FgsSalesDispositionReasonWriteService : IFgsSalesDispositionReasonWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsSalesDispositionReasonWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsSalesDispositionReasonDetailDto> CreateAsync(
        FgsSalesDispositionReasonCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSalesDispositionReason
        {
            DispositionReasonCode = NormalizeCode(dto.DispositionReasonCode),
            DispositionReasonName = dto.DispositionReasonName.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
            DisplayOrder = dto.DisplayOrder,
            IsSystem = dto.IsSystem,
            AppliesToLead = dto.AppliesToLead,
            AppliesToOpportunity = dto.AppliesToOpportunity,
            RequireComment = dto.RequireComment,
            IsTerminal = dto.IsTerminal,
            AllowManualSelection = dto.AllowManualSelection
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSalesDispositionReasons.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSalesDispositionReasonDetailDto> UpdateAsync(
        long id,
        FgsSalesDispositionReasonUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales Disposition Reason '{id}' was not found.");

        entity.DispositionReasonCode = NormalizeCode(dto.DispositionReasonCode);
        entity.DispositionReasonName = dto.DispositionReasonName.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.DisplayOrder = dto.DisplayOrder;
        entity.IsSystem = dto.IsSystem;
        entity.AppliesToLead = dto.AppliesToLead;
        entity.AppliesToOpportunity = dto.AppliesToOpportunity;
        entity.RequireComment = dto.RequireComment;
        entity.IsTerminal = dto.IsTerminal;
        entity.AllowManualSelection = dto.AllowManualSelection;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSalesDispositionReasonDetailDto> PatchAsync(
        long id,
        FgsSalesDispositionReasonPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales Disposition Reason '{id}' was not found.");

        if (dto.DispositionReasonCode is not null)
        {
            entity.DispositionReasonCode = NormalizeCode(dto.DispositionReasonCode); ;
        }
        if (dto.DispositionReasonName is not null)
        {
            entity.DispositionReasonName = dto.DispositionReasonName.Trim(); ;
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
        if (dto.RequireComment.HasValue)
        {
            entity.RequireComment = dto.RequireComment.Value;
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

    public async Task<FgsSalesDispositionReasonDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales Disposition Reason '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSalesDispositionReason?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSalesDispositionReasons.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A sales disposition reason with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSalesDispositionReasonDetailDto MapToDetail(FgsSalesDispositionReason entity) =>
        new(
            entity.Id,
            entity.DispositionReasonCode,
            entity.DispositionReasonName,
            entity.Description,
            entity.DisplayOrder,
            entity.IsSystem,
            entity.AppliesToLead,
            entity.AppliesToOpportunity,
            entity.RequireComment,
            entity.IsTerminal,
            entity.AllowManualSelection,
            entity.IsActive);
}
