using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.SalesActivityOutcomes;

public sealed class FgsSalesActivityOutcomeWriteService : IFgsSalesActivityOutcomeWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsSalesActivityOutcomeWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsSalesActivityOutcomeDetailDto> CreateAsync(
        FgsSalesActivityOutcomeCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSalesActivityOutcome
        {
            OutcomeCode = NormalizeCode(dto.OutcomeCode), OutcomeName = dto.OutcomeName.Trim(), Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(), DisplayOrder = dto.DisplayOrder, IsSystem = dto.IsSystem, AppliesToLead = dto.AppliesToLead, AppliesToOpportunity = dto.AppliesToOpportunity, NextSalesPipelineStatusId = dto.NextSalesPipelineStatusId, IsTerminal = dto.IsTerminal, RequireComment = dto.RequireComment, AllowManualSelection = dto.AllowManualSelection
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSalesActivityOutcomes.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSalesActivityOutcomeDetailDto> UpdateAsync(
        long id,
        FgsSalesActivityOutcomeUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales Activity Outcome '{id}' was not found.");

        entity.OutcomeCode = NormalizeCode(dto.OutcomeCode);
        entity.OutcomeName = dto.OutcomeName.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.DisplayOrder = dto.DisplayOrder;
        entity.IsSystem = dto.IsSystem;
        entity.AppliesToLead = dto.AppliesToLead;
        entity.AppliesToOpportunity = dto.AppliesToOpportunity;
        entity.NextSalesPipelineStatusId = dto.NextSalesPipelineStatusId;
        entity.IsTerminal = dto.IsTerminal;
        entity.RequireComment = dto.RequireComment;
        entity.AllowManualSelection = dto.AllowManualSelection;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSalesActivityOutcomeDetailDto> PatchAsync(
        long id,
        FgsSalesActivityOutcomePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales Activity Outcome '{id}' was not found.");

        if (dto.OutcomeCode is not null)
        {
            entity.OutcomeCode = NormalizeCode(dto.OutcomeCode);;
        }
        if (dto.OutcomeName is not null)
        {
            entity.OutcomeName = dto.OutcomeName.Trim();;
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
        if (dto.NextSalesPipelineStatusId.HasValue)
        {
            entity.NextSalesPipelineStatusId = dto.NextSalesPipelineStatusId.Value;
        }
        if (dto.IsTerminal.HasValue)
        {
            entity.IsTerminal = dto.IsTerminal.Value;
        }
        if (dto.RequireComment.HasValue)
        {
            entity.RequireComment = dto.RequireComment.Value;
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

    public async Task<FgsSalesActivityOutcomeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales Activity Outcome '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSalesActivityOutcome?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSalesActivityOutcomes.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A sales activity outcome with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSalesActivityOutcomeDetailDto MapToDetail(FgsSalesActivityOutcome entity) =>
        new(
            entity.Id,
            entity.OutcomeCode,
            entity.OutcomeName,
            entity.Description,
            entity.DisplayOrder,
            entity.IsSystem,
            entity.AppliesToLead,
            entity.AppliesToOpportunity,
            entity.NextSalesPipelineStatusId,
            entity.IsTerminal,
            entity.RequireComment,
            entity.AllowManualSelection,
            entity.IsActive);
}
