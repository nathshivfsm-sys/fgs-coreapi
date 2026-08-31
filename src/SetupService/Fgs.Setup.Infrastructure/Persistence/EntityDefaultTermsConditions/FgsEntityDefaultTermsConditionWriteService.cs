using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.EntityDefaultTermsConditions;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.EntityDefaultTermsConditions;

public sealed class FgsEntityDefaultTermsConditionWriteService : IFgsEntityDefaultTermsConditionWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsEntityDefaultTermsConditionWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsEntityDefaultTermsConditionDetailDto> CreateAsync(
        FgsEntityDefaultTermsConditionCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsEntityDefaultTermsCondition
        {
            EntityType = dto.EntityType.Trim(),
            TermsConditionId = dto.TermsConditionId
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsEntityDefaultTermsConditions.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return await MapToDetailAsync(entity, cancellationToken);
    }

    public async Task<FgsEntityDefaultTermsConditionDetailDto> UpdateAsync(
        long id,
        FgsEntityDefaultTermsConditionUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Entity default terms condition '{id}' was not found.");

        entity.EntityType = dto.EntityType.Trim();
        entity.TermsConditionId = dto.TermsConditionId;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return await MapToDetailAsync(entity, cancellationToken);
    }

    public async Task<FgsEntityDefaultTermsConditionDetailDto> PatchAsync(
        long id,
        FgsEntityDefaultTermsConditionPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Entity default terms condition '{id}' was not found.");

        if (dto.EntityType is not null)
        {
            entity.EntityType = dto.EntityType.Trim();
        }

        if (dto.TermsConditionId.HasValue)
        {
            entity.TermsConditionId = dto.TermsConditionId.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return await MapToDetailAsync(entity, cancellationToken);
    }

    private async Task<FgsEntityDefaultTermsCondition?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsEntityDefaultTermsConditions.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "A default terms condition for this entity type already exists.",
                ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private async Task<FgsEntityDefaultTermsConditionDetailDto> MapToDetailAsync(
        FgsEntityDefaultTermsCondition entity,
        CancellationToken cancellationToken)
    {
        var terms = await _context.FgsTermsConditions
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == entity.TermsConditionId, cancellationToken);

        return new FgsEntityDefaultTermsConditionDetailDto(
            entity.Id,
            entity.EntityType,
            entity.TermsConditionId,
            terms?.Code,
            terms?.Name,
            terms?.VersionNumber,
            entity.IsActive);
    }
}
