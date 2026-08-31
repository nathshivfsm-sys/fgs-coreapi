using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.RoleMenus;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.RoleMenus;

public sealed class FgsRoleMenuWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext) : IFgsRoleMenuWriteService
{
    public async Task<IReadOnlyList<FgsRoleMenuDetailDto>> SyncAsync(
        FgsRoleMenuSyncDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);

        var roleExists = await context.FgsRoles.AnyAsync(
            r => r.Id == dto.RoleId && r.TenantId == tenantId && r.CompanyId == companyId,
            cancellationToken);
        if (!roleExists)
        {
            throw new KeyNotFoundException($"Role '{dto.RoleId}' was not found.");
        }

        var desiredItems = (dto.Items ?? [])
            .GroupBy(x => x.MenuId)
            .Select(g => g.Last())
            .ToList();

        var existing = await context.FgsRoleMenus
            .Where(x => x.RoleId == dto.RoleId && x.TenantId == tenantId && x.CompanyId == companyId)
            .ToListAsync(cancellationToken);

        var desiredSet = desiredItems.Select(x => x.MenuId).ToHashSet();
        var existingByMenuId = existing.ToDictionary(x => x.MenuId);

        var toRemove = existing.Where(x => !desiredSet.Contains(x.MenuId)).ToList();
        if (toRemove.Count > 0)
        {
            context.FgsRoleMenus.RemoveRange(toRemove);
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

            await context.FgsRoleMenus.AddAsync(
                new FgsRoleMenu
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    RoleId = dto.RoleId,
                    MenuId = item.MenuId,
                    DisplayOrder = item.DisplayOrder,
                    IsActive = item.IsActive,
                    CreatedOn = now,
                    CreatedBy = actor
                },
                cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await context.FgsRoleMenus
            .AsNoTracking()
            .Where(x => x.RoleId == dto.RoleId && x.TenantId == tenantId && x.CompanyId == companyId)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Id)
            .Select(x => new FgsRoleMenuDetailDto(
                x.Id,
                x.RoleId,
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
