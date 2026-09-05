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
    public async Task<FgsUserRoleDetailDto> CreateAsync(
        FgsUserRoleCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        await EnsureUserExistsAsync(dto.UserId, tenantId, companyId, cancellationToken);
        await EnsureRoleExistsAsync(dto.FgsRoleId, tenantId, companyId, cancellationToken);

        var now = DateTimeOffset.UtcNow;
        var entity = new FgsUserRole
        {
            TenantId = tenantId,
            CompanyId = companyId,
            UserId = dto.UserId,
            FgsRoleId = dto.FgsRoleId,
            CreatedOn = now,
            CreatedBy = ResolveActor()
        };

        await context.FgsUserRoles.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsUserRoleDetailDto> UpdateAsync(
        long id,
        FgsUserRoleUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"User-role assignment '{id}' was not found.");

        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        await EnsureRoleExistsAsync(dto.FgsRoleId, tenantId, companyId, cancellationToken);

        entity.FgsRoleId = dto.FgsRoleId;
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsUserRoleDetailDto> PatchAsync(
        long id,
        FgsUserRolePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"User-role assignment '{id}' was not found.");

        if (dto.FgsRoleId.HasValue)
        {
            var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
            await EnsureRoleExistsAsync(dto.FgsRoleId.Value, tenantId, companyId, cancellationToken);
            entity.FgsRoleId = dto.FgsRoleId.Value;
        }

        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<IReadOnlyList<FgsUserRoleDetailDto>> SyncAsync(
        FgsUserRoleSyncDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);

        await EnsureUserExistsAsync(dto.UserId, tenantId, companyId, cancellationToken);

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

        await SaveChangesAsync(cancellationToken);

        return await context.FgsUserRoles
            .AsNoTracking()
            .Where(x => x.UserId == dto.UserId && x.TenantId == tenantId && x.CompanyId == companyId)
            .OrderBy(x => x.Id)
            .Select(x => MapToDetail(x))
            .ToListAsync(cancellationToken);
    }

    private async Task EnsureUserExistsAsync(
        Guid userId,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var userExists = await context.FgsUsers.AnyAsync(
            u => u.Id == userId && u.TenantId == tenantId && u.CompanyId == companyId,
            cancellationToken);
        if (!userExists)
        {
            throw new KeyNotFoundException($"User '{userId}' was not found.");
        }
    }

    private async Task EnsureRoleExistsAsync(
        long roleId,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var roleExists = await context.FgsRoles.AnyAsync(
            r => r.Id == roleId && r.TenantId == tenantId && r.CompanyId == companyId,
            cancellationToken);
        if (!roleExists)
        {
            throw new KeyNotFoundException($"Role '{roleId}' was not found.");
        }
    }

    private async Task<FgsUserRole?> FindEntityAsync(long id, CancellationToken cancellationToken)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        return await context.FgsUserRoles.FirstOrDefaultAsync(
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
                "A user-role assignment with this UserId and FgsRoleId already exists.",
                ex);
        }
    }

    private string ResolveActor() =>
        userContext.Email
        ?? userContext.DisplayName
        ?? userContext.UserId?.ToString()
        ?? "system";

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static FgsUserRoleDetailDto MapToDetail(FgsUserRole entity) =>
        new(entity.Id, entity.UserId, entity.FgsRoleId, entity.CreatedOn, entity.CreatedBy);
}
