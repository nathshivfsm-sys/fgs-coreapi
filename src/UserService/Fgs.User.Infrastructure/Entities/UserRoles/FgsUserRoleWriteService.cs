using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Features.UserRoles.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.UserRoles;

public sealed class FgsUserRoleWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext) : IFgsUserRoleWriteService
{
    public async Task<IReadOnlyList<FgsUserRoleDetailDto>> SyncAsync(
        FgsUserRoleSyncDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);

        var userExists = await context.FgsUsers.AnyAsync(
            u => u.Id == dto.UserId && u.TenantId == tenantId && u.CompanyId == companyId,
            cancellationToken);
        if (!userExists)
        {
            throw new KeyNotFoundException($"User '{dto.UserId}' was not found.");
        }

        var desiredIds = (dto.FgsRoleIds ?? [])
            .Distinct()
            .ToList();

        if (desiredIds.Count > 0)
        {
            var foundIds = await context.FgsRoles
                .Where(r => desiredIds.Contains(r.Id) && r.TenantId == tenantId && r.CompanyId == companyId)
                .Select(r => r.Id)
                .ToListAsync(cancellationToken);
            var missing = desiredIds.Except(foundIds).ToList();
            if (missing.Count > 0)
            {
                throw new KeyNotFoundException($"Role '{missing[0]}' was not found.");
            }
        }

        var existing = await context.FgsUserRoles
            .Where(x => x.UserId == dto.UserId && x.TenantId == tenantId && x.CompanyId == companyId)
            .ToListAsync(cancellationToken);

        var desiredSet = desiredIds.ToHashSet();
        var existingByRoleId = existing.ToDictionary(x => x.FgsRoleId);

        var toRemove = existing.Where(x => !desiredSet.Contains(x.FgsRoleId)).ToList();
        if (toRemove.Count > 0)
        {
            context.FgsUserRoles.RemoveRange(toRemove);
        }

        var actor = ResolveActor();
        var now = DateTimeOffset.UtcNow;
        foreach (var roleId in desiredIds)
        {
            if (existingByRoleId.ContainsKey(roleId))
            {
                continue;
            }

            await context.FgsUserRoles.AddAsync(
                new FgsUserRole
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    UserId = dto.UserId,
                    FgsRoleId = roleId,
                    CreatedOn = now,
                    CreatedBy = actor
                },
                cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await context.FgsUserRoles
            .AsNoTracking()
            .Where(x => x.UserId == dto.UserId && x.TenantId == tenantId && x.CompanyId == companyId)
            .OrderBy(x => x.Id)
            .Select(x => new FgsUserRoleDetailDto(x.Id, x.UserId, x.FgsRoleId, x.CreatedOn, x.CreatedBy))
            .ToListAsync(cancellationToken);
    }

    private string ResolveActor() =>
        userContext.Email
        ?? userContext.DisplayName
        ?? userContext.UserId?.ToString()
        ?? "system";
}
