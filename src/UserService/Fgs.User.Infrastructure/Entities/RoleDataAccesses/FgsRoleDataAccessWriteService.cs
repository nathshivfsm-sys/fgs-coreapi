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
    public async Task<FgsRoleDataAccessDetailDto> CreateAsync(
        FgsRoleDataAccessCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        await EnsureRoleExistsAsync(dto.FgsRoleId, tenantId, companyId, cancellationToken);
        await EnsureDataAccessExistsAsync(dto.FgsDataAccessId, tenantId, companyId, cancellationToken);

        var entity = new FgsRoleDataAccess
        {
            TenantId = tenantId,
            CompanyId = companyId,
            FgsRoleId = dto.FgsRoleId,
            FgsDataAccessId = dto.FgsDataAccessId,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = ResolveActor()
        };

        await context.FgsRoleDataAccesses.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsRoleDataAccessDetailDto> UpdateAsync(
        long id,
        FgsRoleDataAccessUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Role-data-access assignment '{id}' was not found.");

        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        await EnsureDataAccessExistsAsync(dto.FgsDataAccessId, tenantId, companyId, cancellationToken);

        entity.FgsDataAccessId = dto.FgsDataAccessId;
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsRoleDataAccessDetailDto> PatchAsync(
        long id,
        FgsRoleDataAccessPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Role-data-access assignment '{id}' was not found.");

        if (dto.FgsDataAccessId.HasValue)
        {
            var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
            await EnsureDataAccessExistsAsync(dto.FgsDataAccessId.Value, tenantId, companyId, cancellationToken);
            entity.FgsDataAccessId = dto.FgsDataAccessId.Value;
        }

        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<IReadOnlyList<FgsRoleDataAccessDetailDto>> SyncAsync(
        FgsRoleDataAccessSyncDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);

        await EnsureRoleExistsAsync(dto.FgsRoleId, tenantId, companyId, cancellationToken);

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

        await SaveChangesAsync(cancellationToken);

        return await context.FgsRoleDataAccesses
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

    private async Task EnsureDataAccessExistsAsync(
        long dataAccessId,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var dataAccessExists = await context.FgsDataAccesses.AnyAsync(
            d => d.Id == dataAccessId && d.TenantId == tenantId && d.CompanyId == companyId,
            cancellationToken);
        if (!dataAccessExists)
        {
            throw new KeyNotFoundException($"Data access '{dataAccessId}' was not found.");
        }
    }

    private async Task<FgsRoleDataAccess?> FindEntityAsync(long id, CancellationToken cancellationToken)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        return await context.FgsRoleDataAccesses.FirstOrDefaultAsync(
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
                "A role-data-access assignment with this FgsRoleId and FgsDataAccessId already exists.",
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

    private static FgsRoleDataAccessDetailDto MapToDetail(FgsRoleDataAccess entity) =>
        new(entity.Id, entity.FgsRoleId, entity.FgsDataAccessId, entity.CreatedOn, entity.CreatedBy);
}
