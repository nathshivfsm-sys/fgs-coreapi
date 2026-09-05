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
    public async Task<FgsRolePermissionDetailDto> CreateAsync(
        FgsRolePermissionCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        await EnsureRoleExistsAsync(dto.FgsRoleId, tenantId, companyId, cancellationToken);
        await EnsurePermissionExistsAsync(dto.FgsPermissionId, cancellationToken);

        var entity = new FgsRolePermission
        {
            TenantId = tenantId,
            CompanyId = companyId,
            FgsRoleId = dto.FgsRoleId,
            FgsPermissionId = dto.FgsPermissionId,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = ResolveActor()
        };

        await context.FgsRolePermissions.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsRolePermissionDetailDto> UpdateAsync(
        long id,
        FgsRolePermissionUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Role-permission assignment '{id}' was not found.");

        await EnsurePermissionExistsAsync(dto.FgsPermissionId, cancellationToken);

        entity.FgsPermissionId = dto.FgsPermissionId;
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsRolePermissionDetailDto> PatchAsync(
        long id,
        FgsRolePermissionPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Role-permission assignment '{id}' was not found.");

        if (dto.FgsPermissionId.HasValue)
        {
            await EnsurePermissionExistsAsync(dto.FgsPermissionId.Value, cancellationToken);
            entity.FgsPermissionId = dto.FgsPermissionId.Value;
        }

        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<IReadOnlyList<FgsRolePermissionDetailDto>> SyncAsync(
        FgsRolePermissionSyncDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);

        await EnsureRoleExistsAsync(dto.FgsRoleId, tenantId, companyId, cancellationToken);

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

        await SaveChangesAsync(cancellationToken);

        return await context.FgsRolePermissions
            .AsNoTracking()
            .Where(x => x.FgsRoleId == dto.FgsRoleId && x.TenantId == tenantId && x.CompanyId == companyId)
            .OrderBy(x => x.Id)
            .Select(x => MapToDetail(x))
            .ToListAsync(cancellationToken);
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

    private async Task EnsurePermissionExistsAsync(long permissionId, CancellationToken cancellationToken)
    {
        var permissionExists = await context.FgsPermissions.AnyAsync(
            p => p.Id == permissionId,
            cancellationToken);
        if (!permissionExists)
        {
            throw new KeyNotFoundException($"Permission '{permissionId}' was not found.");
        }
    }

    private async Task<FgsRolePermission?> FindEntityAsync(long id, CancellationToken cancellationToken)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        return await context.FgsRolePermissions.FirstOrDefaultAsync(
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
                "A role-permission assignment with this FgsRoleId and FgsPermissionId already exists.",
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

    private static FgsRolePermissionDetailDto MapToDetail(FgsRolePermission entity) =>
        new(entity.Id, entity.FgsRoleId, entity.FgsPermissionId, entity.CreatedOn, entity.CreatedBy);
}
