using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.RoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.RoleDataAccesses;

public sealed class FgsRoleDataAccessWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext) : IFgsRoleDataAccessWriteService
{
    public async Task<IReadOnlyList<FgsRoleDataAccessDetailDto>> SyncAsync(
        FgsRoleDataAccessSyncDto dto,
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

        var desiredIds = (dto.FgsDataAccessIds ?? [])
            .Distinct()
            .ToList();

        if (desiredIds.Count > 0)
        {
            var foundIds = await context.FgsDataAccesses
                .Where(d => desiredIds.Contains(d.Id) && d.TenantId == tenantId && d.CompanyId == companyId)
                .Select(d => d.Id)
                .ToListAsync(cancellationToken);
            var missing = desiredIds.Except(foundIds).ToList();
            if (missing.Count > 0)
            {
                throw new KeyNotFoundException($"Data access '{missing[0]}' was not found.");
            }
        }

        var existing = await context.FgsRoleDataAccesses
            .Where(x => x.FgsRoleId == dto.FgsRoleId && x.TenantId == tenantId && x.CompanyId == companyId)
            .ToListAsync(cancellationToken);

        var desiredSet = desiredIds.ToHashSet();
        var existingByDataAccessId = existing.ToDictionary(x => x.FgsDataAccessId);

        var toRemove = existing.Where(x => !desiredSet.Contains(x.FgsDataAccessId)).ToList();
        if (toRemove.Count > 0)
        {
            context.FgsRoleDataAccesses.RemoveRange(toRemove);
        }

        var actor = ResolveActor();
        var now = DateTimeOffset.UtcNow;
        foreach (var dataAccessId in desiredIds)
        {
            if (existingByDataAccessId.ContainsKey(dataAccessId))
            {
                continue;
            }

            await context.FgsRoleDataAccesses.AddAsync(
                new FgsRoleDataAccess
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    FgsRoleId = dto.FgsRoleId,
                    FgsDataAccessId = dataAccessId,
                    CreatedOn = now,
                    CreatedBy = actor
                },
                cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await context.FgsRoleDataAccesses
            .AsNoTracking()
            .Where(x => x.FgsRoleId == dto.FgsRoleId && x.TenantId == tenantId && x.CompanyId == companyId)
            .OrderBy(x => x.Id)
            .Select(x => new FgsRoleDataAccessDetailDto(
                x.Id,
                x.FgsRoleId,
                x.FgsDataAccessId,
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
