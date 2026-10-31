using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupPriceBookVersions;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Entities.SetupPriceBookVersions;

public sealed class FgsSetupPriceBookVersionWriteService(
    FgsSetupDbContext context,
    IUnitOfWork unitOfWork,
    SetupEntityAuditHelper auditHelper) : IFgsSetupPriceBookVersionWriteService
{
    public async Task<FgsSetupPriceBookVersionDetailDto> CreateAsync(
        FgsSetupPriceBookVersionCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupPriceBookVersion
        {
            Version = dto.Version.Trim(),
            EffectiveFrom = dto.EffectiveFrom,
            EffectiveTo = dto.EffectiveTo
        };

        auditHelper.StampForCreate(entity);
        await context.FgsSetupPriceBookVersions.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupPriceBookVersionDetailDto> UpdateAsync(
        long id,
        FgsSetupPriceBookVersionUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Price book version '{id}' was not found.");

        entity.Version = dto.Version.Trim();
        entity.EffectiveFrom = dto.EffectiveFrom;
        entity.EffectiveTo = dto.EffectiveTo;

        auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupPriceBookVersionDetailDto> PatchAsync(
        long id,
        FgsSetupPriceBookVersionPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Price book version '{id}' was not found.");

        if (dto.Version is not null)
        {
            entity.Version = dto.Version.Trim();
        }

        if (dto.EffectiveFrom.HasValue)
        {
            entity.EffectiveFrom = dto.EffectiveFrom.Value;
        }

        if (dto.EffectiveTo.HasValue)
        {
            entity.EffectiveTo = dto.EffectiveTo;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    private async Task<FgsSetupPriceBookVersion?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await context.FgsSetupPriceBookVersions.FirstOrDefaultIncludingInactiveAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "A price book version with this version already exists for the company.",
                ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("UQ_FgsSetupPriceBookVersion", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static FgsSetupPriceBookVersionDetailDto MapToDetail(FgsSetupPriceBookVersion entity) =>
        new(entity.Id, entity.Version, entity.EffectiveFrom, entity.EffectiveTo, entity.IsActive);
}
