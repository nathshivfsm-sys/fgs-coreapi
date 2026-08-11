using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.NonWorkingDates;
using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.NonWorkingDates;

public sealed class FgsNonWorkingDateWriteService(
    FgsSetupDbContext context,
    IUnitOfWork unitOfWork,
    SetupEntityAuditHelper auditHelper) : IFgsNonWorkingDateWriteService
{
    public async Task<FgsNonWorkingDateDetailDto> CreateAsync(
        FgsNonWorkingDateCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsNonWorkingDate
        {
            NonWorkingDate = dto.NonWorkingDate,
            Name = dto.Name.Trim()
        };

        auditHelper.StampForCreate(entity);
        await context.FgsNonWorkingDates.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsNonWorkingDateDetailDto> UpdateAsync(
        long id,
        FgsNonWorkingDateUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Non-working date '{id}' was not found.");

        entity.NonWorkingDate = dto.NonWorkingDate;
        entity.Name = dto.Name.Trim();

        auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsNonWorkingDateDetailDto> PatchAsync(
        long id,
        FgsNonWorkingDatePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Non-working date '{id}' was not found.");

        if (dto.NonWorkingDate.HasValue)
        {
            entity.NonWorkingDate = dto.NonWorkingDate.Value;
        }

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    private async Task<FgsNonWorkingDate?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await context.FgsNonWorkingDates.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "A non-working date with this calendar date already exists for the company.",
                ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("UQ_FgsNonWorkingDate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) == true;

    private static FgsNonWorkingDateDetailDto MapToDetail(FgsNonWorkingDate entity) =>
        new(entity.Id, entity.NonWorkingDate, entity.Name, entity.IsActive);
}
