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
                existingRow.DisplayOrder = item.DisplayOrder;
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
                    DisplayOrder = item.DisplayOrder,
                    IsActive = item.IsActive,
                    CreatedOn = now,
                    CreatedBy = actor
                },
                cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await context.FgsTenantMenus
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && x.CompanyId == companyId)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Id)
            .Select(x => new FgsTenantMenuDetailDto(
                x.Id,
                x.MenuId,
                x.DisplayOrder,
                x.IsActive,
                x.CreatedOn,
                x.CreatedBy))
            .ToListAsync(cancellationToken);
    }

    private string ResolveActor() =>
        userContext.Email
        ?? userContext.DisplayName
        ?? userContext.UserId?.ToString()
        ?? "system";
}
