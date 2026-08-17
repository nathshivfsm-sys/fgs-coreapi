using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.RolePermissions;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.RolePermissions;

public sealed class FgsRolePermissionWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext) : IFgsRolePermissionWriteService
{
    public async Task<IReadOnlyList<FgsRolePermissionDetailDto>> SyncAsync(
        FgsRolePermissionSyncDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);

        var roleExists = await context.FgsRoles.AnyAsync(
            r => r.Id == dto.FgsRoleId && r.TenantId == tenantId && r.CompanyId == companyId,
            cancellationToken);
        if (!roleExists)
        {
            throw new KeyNotFoundException($"Role '{dto.FgsRoleId}' was not found.");
        }

        var desiredIds = (dto.FgsPermissionIds ?? [])
            .Distinct()
            .ToList();

        if (desiredIds.Count > 0)
        {
            var foundPermissionIds = await context.FgsPermissions
                .Where(p => desiredIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync(cancellationToken);
            var missing = desiredIds.Except(foundPermissionIds).ToList();
            if (missing.Count > 0)
            {
                throw new KeyNotFoundException($"Permission '{missing[0]}' was not found.");
            }
        }

        var existing = await context.FgsRolePermissions
            .Where(x => x.FgsRoleId == dto.FgsRoleId && x.TenantId == tenantId && x.CompanyId == companyId)
            .ToListAsync(cancellationToken);

        var desiredSet = desiredIds.ToHashSet();
        var existingByPermissionId = existing.ToDictionary(x => x.FgsPermissionId);

        var toRemove = existing.Where(x => !desiredSet.Contains(x.FgsPermissionId)).ToList();
        if (toRemove.Count > 0)
        {
            context.FgsRolePermissions.RemoveRange(toRemove);
        }

        var actor = ResolveActor();
        var now = DateTimeOffset.UtcNow;
        foreach (var permissionId in desiredIds)
        {
            if (existingByPermissionId.ContainsKey(permissionId))
            {
                continue;
            }

            await context.FgsRolePermissions.AddAsync(
                new FgsRolePermission
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    FgsRoleId = dto.FgsRoleId,
                    FgsPermissionId = permissionId,
                    CreatedOn = now,
                    CreatedBy = actor
                },
                cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await context.FgsRolePermissions
            .AsNoTracking()
            .Where(x => x.FgsRoleId == dto.FgsRoleId && x.TenantId == tenantId && x.CompanyId == companyId)
            .OrderBy(x => x.Id)
            .Select(x => new FgsRolePermissionDetailDto(
                x.Id,
                x.FgsRoleId,
                x.FgsPermissionId,
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
