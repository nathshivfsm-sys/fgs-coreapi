using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.LeadDisqualificationReasons;

public sealed class LeadDisqualificationReasonWriteService : ILeadDisqualificationReasonWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public LeadDisqualificationReasonWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<LeadDisqualificationReasonDetailDto> CreateAsync(
        LeadDisqualificationReasonCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsLeadDisqualificationReason
        {
            ReasonCode = NormalizeCode(dto.ReasonCode), ReasonName = dto.ReasonName.Trim(), Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(), DisplayOrder = dto.DisplayOrder ?? 1, IsSystem = dto.IsSystem
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsLeadDisqualificationReasons.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<LeadDisqualificationReasonDetailDto> UpdateAsync(
        long id,
        LeadDisqualificationReasonUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Lead Disqualification Reason '{id}' was not found.");

        entity.ReasonCode = NormalizeCode(dto.ReasonCode);
        entity.ReasonName = dto.ReasonName.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.DisplayOrder = dto.DisplayOrder ?? entity.DisplayOrder;
        entity.IsSystem = dto.IsSystem;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<LeadDisqualificationReasonDetailDto> PatchAsync(
        long id,
        LeadDisqualificationReasonPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Lead Disqualification Reason '{id}' was not found.");

        if (dto.ReasonCode is not null)
        {
            entity.ReasonCode = NormalizeCode(dto.ReasonCode);;
        }
        if (dto.ReasonName is not null)
        {
            entity.ReasonName = dto.ReasonName.Trim();;
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

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<LeadDisqualificationReasonDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Lead Disqualification Reason '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsLeadDisqualificationReason?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsLeadDisqualificationReasons.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A lead disqualification reason with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static LeadDisqualificationReasonDetailDto MapToDetail(FgsLeadDisqualificationReason entity) =>
        new(
            entity.Id,
            entity.TenantId,
            entity.CompanyId,
            entity.ReasonCode,
            entity.ReasonName,
            entity.Description,
            entity.DisplayOrder,
            entity.IsSystem,
            entity.IsActive,
            entity.CreatedOn,
            entity.CreatedBy,
            entity.UpdatedOn,
            entity.UpdatedBy);
}
