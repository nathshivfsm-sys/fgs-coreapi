using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.PriceBooks;
using Fgs.Setup.Application.Features.PriceBooks.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.PriceBooks;

public sealed class FgsPriceBookWriteService(
    FgsSetupDbContext context,
    IUnitOfWork unitOfWork,
    SetupEntityAuditHelper auditHelper) : IFgsPriceBookWriteService
{
    public async Task<FgsPriceBookDetailDto> CreateAsync(
        FgsPriceBookCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsPriceBook
        {
            PriceBookCode = dto.PriceBookCode.Trim(),
            PriceBookName = dto.PriceBookName.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
            JobTypeId = dto.JobTypeId,
            PricingModel = dto.PricingModel.Trim(),
            EstimatedDurationMinutes = dto.EstimatedDurationMinutes,
            BasePrice = dto.PricingModel == PriceBookPricingModels.Dynamic ? null : dto.BasePrice,
            IsTaxable = dto.IsTaxable
        };

        auditHelper.StampForCreate(entity);
        await context.FgsPriceBooks.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsPriceBookDetailDto> UpdateAsync(
        long id,
        FgsPriceBookUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Price book '{id}' was not found.");

        entity.PriceBookCode = dto.PriceBookCode.Trim();
        entity.PriceBookName = dto.PriceBookName.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.JobTypeId = dto.JobTypeId;
        entity.PricingModel = dto.PricingModel.Trim();
        entity.EstimatedDurationMinutes = dto.EstimatedDurationMinutes;
        entity.BasePrice = dto.PricingModel == PriceBookPricingModels.Dynamic ? null : dto.BasePrice;
        entity.IsTaxable = dto.IsTaxable;

        auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsPriceBookDetailDto> PatchAsync(
        long id,
        FgsPriceBookPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Price book '{id}' was not found.");

        if (dto.PriceBookCode is not null)
        {
            entity.PriceBookCode = dto.PriceBookCode.Trim();
        }

        if (dto.PriceBookName is not null)
        {
            entity.PriceBookName = dto.PriceBookName.Trim();
        }

        if (dto.Description is not null)
        {
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        }

        if (dto.JobTypeId.HasValue)
        {
            entity.JobTypeId = dto.JobTypeId.Value;
        }

        if (dto.PricingModel is not null)
        {
            entity.PricingModel = dto.PricingModel.Trim();
            if (entity.PricingModel == PriceBookPricingModels.Dynamic)
            {
                entity.BasePrice = null;
            }
        }

        if (dto.EstimatedDurationMinutes.HasValue)
        {
            entity.EstimatedDurationMinutes = dto.EstimatedDurationMinutes.Value;
        }

        if (dto.BasePrice.HasValue)
        {
            entity.BasePrice = dto.BasePrice;
        }
        else if (dto.PricingModel == PriceBookPricingModels.Dynamic)
        {
            entity.BasePrice = null;
        }

        if (dto.IsTaxable.HasValue)
        {
            entity.IsTaxable = dto.IsTaxable.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    private async Task<FgsPriceBook?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await context.FgsPriceBooks.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "A price book with this code already exists for the company.",
                ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("UX_FgsPriceBook", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) == true;

    private static FgsPriceBookDetailDto MapToDetail(FgsPriceBook entity) =>
        new(
            entity.Id,
            entity.PriceBookCode,
            entity.PriceBookName,
            entity.Description,
            entity.JobTypeId,
            entity.PricingModel,
            entity.EstimatedDurationMinutes,
            entity.BasePrice,
            entity.IsTaxable,
            entity.IsActive);
}
