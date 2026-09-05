using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.TenantMenus;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.TenantMenus;

public sealed class FgsTenantMenuWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext) : IFgsTenantMenuWriteService
{
    public async Task<FgsTenantMenuDetailDto> CreateAsync(
        FgsTenantMenuCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var now = DateTimeOffset.UtcNow;
        var actor = ResolveActor();

        var entity = new FgsTenantMenu
        {
            TenantId = tenantId,
            CompanyId = companyId,
            MenuId = dto.MenuId,
            MenuCode = dto.MenuCode.Trim(),
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim(),
            ParentMenuId = dto.ParentMenuId,
            MenuType = dto.MenuType.Trim(),
            Route = dto.Route?.Trim(),
            Icon = dto.Icon?.Trim(),
            DisplayOrder = dto.DisplayOrder,
            IsActive = true,
            CreatedOn = now,
            CreatedBy = actor
        };

        await context.FgsTenantMenus.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsTenantMenuDetailDto> UpdateAsync(
        long id,
        FgsTenantMenuUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tenant menu '{id}' was not found.");

        ApplyCatalogFields(
            entity,
            dto.MenuId,
            dto.MenuCode,
            dto.Name,
            dto.Description,
            dto.ParentMenuId,
            dto.MenuType,
            dto.Route,
            dto.Icon,
            dto.DisplayOrder);
        StampForUpdate(entity);

        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsTenantMenuDetailDto> PatchAsync(
        long id,
        FgsTenantMenuPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tenant menu '{id}' was not found.");

        if (dto.MenuId.HasValue)
        {
            entity.MenuId = dto.MenuId.Value;
        }

        if (dto.MenuCode is not null)
        {
            entity.MenuCode = dto.MenuCode.Trim();
        }

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.Description is not null)
        {
            entity.Description = dto.Description.Trim();
        }

        if (dto.ParentMenuId.HasValue)
        {
            entity.ParentMenuId = dto.ParentMenuId;
        }

        if (dto.MenuType is not null)
        {
            entity.MenuType = dto.MenuType.Trim();
        }

        if (dto.Route is not null)
        {
            entity.Route = dto.Route.Trim();
        }

        if (dto.Icon is not null)
        {
            entity.Icon = dto.Icon.Trim();
        }

        if (dto.DisplayOrder.HasValue)
        {
            entity.DisplayOrder = dto.DisplayOrder.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<IReadOnlyList<FgsTenantMenuDetailDto>> SyncAsync(
        FgsTenantMenuSyncDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);

        var desiredItems = (dto.Items ?? [])
            .GroupBy(x => x.MenuId)
            .Select(g => g.Last())
            .ToList();

        var existing = await context.FgsTenantMenus
            .Where(x => x.TenantId == tenantId && x.CompanyId == companyId)
            .ToListAsync(cancellationToken);

        var desiredSet = desiredItems.Select(x => x.MenuId).ToHashSet();
        var existingByMenuId = existing.ToDictionary(x => x.MenuId);

        var toRemove = existing.Where(x => !desiredSet.Contains(x.MenuId)).ToList();
        if (toRemove.Count > 0)
        {
            context.FgsTenantMenus.RemoveRange(toRemove);
        }

        var actor = ResolveActor();
        var now = DateTimeOffset.UtcNow;
        foreach (var item in desiredItems)
        {
            if (existingByMenuId.TryGetValue(item.MenuId, out var existingRow))
            {
                ApplyCatalogFields(
                    existingRow,
                    item.MenuId,
                    item.MenuCode,
                    item.Name,
                    item.Description,
                    item.ParentMenuId,
                    item.MenuType,
                    item.Route,
                    item.Icon,
                    item.DisplayOrder);
                existingRow.IsActive = item.IsActive;
                existingRow.UpdatedOn = now;
                existingRow.UpdatedBy = actor;
                continue;
            }

            await context.FgsTenantMenus.AddAsync(
                new FgsTenantMenu
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    MenuId = item.MenuId,
                    MenuCode = item.MenuCode.Trim(),
                    Name = item.Name.Trim(),
                    Description = item.Description?.Trim(),
                    ParentMenuId = item.ParentMenuId,
                    MenuType = item.MenuType.Trim(),
                    Route = item.Route?.Trim(),
                    Icon = item.Icon?.Trim(),
                    DisplayOrder = item.DisplayOrder,
                    IsActive = item.IsActive,
                    CreatedOn = now,
                    CreatedBy = actor
                },
                cancellationToken);
        }

        await SaveChangesAsync(cancellationToken);

        return await context.FgsTenantMenus
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && x.CompanyId == companyId)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Id)
            .Select(x => MapToDetail(x))
            .ToListAsync(cancellationToken);
    }

    private async Task<FgsTenantMenu?> FindEntityAsync(long id, CancellationToken cancellationToken)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        return await context.FgsTenantMenus.FirstOrDefaultAsync(
            x => x.Id == id && x.TenantId == tenantId && x.CompanyId == companyId,
            cancellationToken);
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "A tenant menu with the same MenuId or MenuCode already exists.",
                ex);
        }
    }

    private void StampForUpdate(FgsTenantMenu entity)
    {
        entity.UpdatedOn = DateTimeOffset.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    private string ResolveActor() =>
        userContext.Email
        ?? userContext.DisplayName
        ?? userContext.UserId?.ToString()
        ?? "system";

    private static void ApplyCatalogFields(
        FgsTenantMenu entity,
        int menuId,
        string menuCode,
        string name,
        string? description,
        int? parentMenuId,
        string menuType,
        string? route,
        string? icon,
        short displayOrder)
    {
        entity.MenuId = menuId;
        entity.MenuCode = menuCode.Trim();
        entity.Name = name.Trim();
        entity.Description = description?.Trim();
        entity.ParentMenuId = parentMenuId;
        entity.MenuType = menuType.Trim();
        entity.Route = route?.Trim();
        entity.Icon = icon?.Trim();
        entity.DisplayOrder = displayOrder;
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static FgsTenantMenuDetailDto MapToDetail(FgsTenantMenu entity) =>
        new(
            entity.Id,
            entity.MenuId,
            entity.MenuCode,
            entity.Name,
            entity.Description,
            entity.ParentMenuId,
            entity.MenuType,
            entity.Route,
            entity.Icon,
            entity.DisplayOrder,
            entity.IsActive,
            entity.CreatedOn,
            entity.CreatedBy);
}
