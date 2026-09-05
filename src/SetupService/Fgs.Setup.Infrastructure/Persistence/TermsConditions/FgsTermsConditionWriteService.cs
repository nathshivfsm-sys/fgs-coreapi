using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.TermsConditions;
using Fgs.Setup.Application.Features.TermsConditions.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.TermsConditions;

public sealed class FgsTermsConditionWriteService : IFgsTermsConditionWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsTermsConditionWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsTermsConditionDetailDto> CreateAsync(
        FgsTermsConditionCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsTermsCondition
        {
            Code = dto.Code.Trim(),
            Name = dto.Name.Trim(),
            VersionNumber = dto.VersionNumber,
            TermsText = dto.TermsText.Trim()
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsTermsConditions.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsTermsConditionDetailDto> UpdateAsync(
        long id,
        FgsTermsConditionUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Terms condition '{id}' was not found.");

        entity.Code = dto.Code.Trim();
        entity.Name = dto.Name.Trim();
        entity.VersionNumber = dto.VersionNumber;
        entity.TermsText = dto.TermsText.Trim();

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsTermsConditionDetailDto> PatchAsync(
        long id,
        FgsTermsConditionPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Terms condition '{id}' was not found.");

        if (dto.Code is not null)
        {
            entity.Code = dto.Code.Trim();
        }

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.VersionNumber.HasValue)
        {
            entity.VersionNumber = dto.VersionNumber.Value;
        }

        if (dto.TermsText is not null)
        {
            entity.TermsText = dto.TermsText.Trim();
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    private async Task<FgsTermsCondition?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsTermsConditions.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "A terms condition with the same code and version already exists.",
                ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static FgsTermsConditionDetailDto MapToDetail(FgsTermsCondition entity) =>
        new(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.VersionNumber,
            entity.TermsText,
            entity.IsActive);
}
