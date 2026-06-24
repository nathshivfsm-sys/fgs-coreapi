using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.LeadStatuses;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.LeadStatuses;

public sealed class LeadStatusWriteService : ILeadStatusWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public LeadStatusWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<LeadStatusDetailDto> CreateAsync(
        LeadStatusCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsLeadStatus
        {
            StatusCode = NormalizeCode(dto.StatusCode), StatusName = dto.StatusName.Trim(), Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(), DisplayOrder = dto.DisplayOrder ?? 1, IsSystem = dto.IsSystem
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsLeadStatuses.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<LeadStatusDetailDto> UpdateAsync(
        long id,
        LeadStatusUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Lead Status '{id}' was not found.");

        entity.StatusCode = NormalizeCode(dto.StatusCode);
        entity.StatusName = dto.StatusName.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.DisplayOrder = dto.DisplayOrder ?? entity.DisplayOrder;
        entity.IsSystem = dto.IsSystem;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<LeadStatusDetailDto> PatchAsync(
        long id,
        LeadStatusPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Lead Status '{id}' was not found.");

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

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<LeadStatusDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Lead Status '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsLeadStatus?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsLeadStatuses.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A lead status with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static LeadStatusDetailDto MapToDetail(FgsLeadStatus entity) =>
        new(
            entity.Id,
            entity.TenantId,
            entity.CompanyId,
            entity.StatusCode,
            entity.StatusName,
            entity.Description,
            entity.DisplayOrder,
            entity.IsSystem,
            entity.IsActive,
            entity.CreatedOn,
            entity.CreatedBy,
            entity.UpdatedOn,
            entity.UpdatedBy);
}
