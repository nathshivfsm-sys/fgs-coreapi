using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.PriceBookItems;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.PriceBookItems;

public sealed class FgsPriceBookItemWriteService(
    FgsSetupDbContext context,
    IUnitOfWork unitOfWork,
    SetupEntityAuditHelper auditHelper) : IFgsPriceBookItemWriteService
{
    public async Task<FgsPriceBookItemDetailDto> CreateAsync(
        FgsPriceBookItemCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsPriceBookItem
        {
            PriceBookId = dto.PriceBookId,
            InventoryItemId = dto.InventoryItemId,
            ItemCode = string.IsNullOrWhiteSpace(dto.ItemCode) ? null : dto.ItemCode.Trim(),
            ItemDescription = dto.ItemDescription.Trim(),
            Quantity = dto.Quantity,
            DisplayOrder = dto.DisplayOrder,
            Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim()
        };

        auditHelper.StampForCreate(entity);
        await context.FgsPriceBookItems.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsPriceBookItemDetailDto> UpdateAsync(
        long id,
        FgsPriceBookItemUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Price book item '{id}' was not found.");

        entity.PriceBookId = dto.PriceBookId;
        entity.InventoryItemId = dto.InventoryItemId;
        entity.ItemCode = string.IsNullOrWhiteSpace(dto.ItemCode) ? null : dto.ItemCode.Trim();
        entity.ItemDescription = dto.ItemDescription.Trim();
        entity.Quantity = dto.Quantity;
        entity.DisplayOrder = dto.DisplayOrder;
        entity.Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();

        auditHelper.StampForUpdate(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsPriceBookItemDetailDto> PatchAsync(
        long id,
        FgsPriceBookItemPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Price book item '{id}' was not found.");

        if (dto.PriceBookId.HasValue)
        {
            entity.PriceBookId = dto.PriceBookId.Value;
        }

        if (dto.InventoryItemId.HasValue)
        {
            entity.InventoryItemId = dto.InventoryItemId;
        }

        if (dto.ItemCode is not null)
        {
            entity.ItemCode = string.IsNullOrWhiteSpace(dto.ItemCode) ? null : dto.ItemCode.Trim();
        }

        if (dto.ItemDescription is not null)
        {
            entity.ItemDescription = dto.ItemDescription.Trim();
        }

        if (dto.Quantity.HasValue)
        {
            entity.Quantity = dto.Quantity.Value;
        }

        if (dto.DisplayOrder.HasValue)
        {
            entity.DisplayOrder = dto.DisplayOrder.Value;
        }

        if (dto.Notes is not null)
        {
            entity.Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();
        }

        auditHelper.StampForUpdate(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsPriceBookItemDetailDto> DeleteAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Price book item '{id}' was not found.");

        var detail = MapToDetail(entity);
        context.FgsPriceBookItems.Remove(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return detail;
    }

    private async Task<FgsPriceBookItem?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await context.FgsPriceBookItems.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private static FgsPriceBookItemDetailDto MapToDetail(FgsPriceBookItem entity) =>
        new(
            entity.Id,
            entity.PriceBookId,
            entity.InventoryItemId,
            entity.ItemCode,
            entity.ItemDescription,
            entity.Quantity,
            entity.DisplayOrder,
            entity.Notes);
}
