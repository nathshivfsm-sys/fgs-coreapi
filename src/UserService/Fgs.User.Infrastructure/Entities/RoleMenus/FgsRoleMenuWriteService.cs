using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.RoleMenus;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Fgs.MultiTenancy.Persistence;

namespace Fgs.User.Infrastructure.Entities.RoleMenus;

public sealed class FgsRoleMenuWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext,
    IFgsRoleMenuReadRepository readRepository) : IFgsRoleMenuWriteService
{
    public async Task<FgsRoleMenuDetailDto> CreateAsync(
        FgsRoleMenuCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        await EnsureRoleExistsAsync(dto.RoleId, tenantId, companyId, cancellationToken);

        var now = DateTimeOffset.UtcNow;
        var actor = ResolveActor();
        var entity = new FgsRoleMenu
        {
            TenantId = tenantId,
            CompanyId = companyId,
            RoleId = dto.RoleId,
            MenuId = dto.MenuId,
            DisplayOrder = dto.DisplayOrder ?? await DisplayOrderSequence.NextAsync(
                context.FgsRoleMenus.Where(t => t.RoleId == dto.RoleId),
                t => t.DisplayOrder,
                cancellationToken),
            IsActive = true,
            CreatedOn = now,
            CreatedBy = actor
        };

        await context.FgsRoleMenus.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return await ReloadAsync(entity.Id, cancellationToken);
    }

    public async Task<FgsRoleMenuDetailDto> UpdateAsync(
        long id,
        FgsRoleMenuUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var entity = await FindEntityAsync(id, tenantId, companyId, cancellationToken)
            ?? throw new KeyNotFoundException($"Role menu '{id}' was not found.");

        await EnsureRoleExistsAsync(dto.RoleId, tenantId, companyId, cancellationToken);

        entity.RoleId = dto.RoleId;
        entity.MenuId = dto.MenuId;
        entity.DisplayOrder = dto.DisplayOrder;
        StampForUpdate(entity);

        await SaveChangesAsync(cancellationToken);
        return await ReloadAsync(entity.Id, cancellationToken);
    }

    public async Task<FgsRoleMenuDetailDto> PatchAsync(
        long id,
        FgsRoleMenuPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var entity = await FindEntityAsync(id, tenantId, companyId, cancellationToken)
            ?? throw new KeyNotFoundException($"Role menu '{id}' was not found.");

        if (dto.RoleId.HasValue)
        {
            await EnsureRoleExistsAsync(dto.RoleId.Value, tenantId, companyId, cancellationToken);
            entity.RoleId = dto.RoleId.Value;
        }

        if (dto.MenuId.HasValue)
        {
            entity.MenuId = dto.MenuId.Value;
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
        return await ReloadAsync(entity.Id, cancellationToken);
    }

    public async Task<IReadOnlyList<FgsRoleMenuDetailDto>> SyncAsync(
        FgsRoleMenuSyncDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        await EnsureRoleExistsAsync(dto.RoleId, tenantId, companyId, cancellationToken);

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
        var nextOrder = await DisplayOrderSequence.NextAsync(
            context.FgsRoleMenus.Where(menu => menu.RoleId == dto.RoleId),
            menu => menu.DisplayOrder,
            cancellationToken);
        var sequenceFull = false;
        foreach (var item in desiredItems)
        {
            if (existingByMenuId.TryGetValue(item.MenuId, out var existingRow))
            {
                if (item.DisplayOrder.HasValue)
                {
                    existingRow.DisplayOrder = item.DisplayOrder.Value;
                }

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
                    DisplayOrder = item.DisplayOrder ?? DisplayOrderSequence.TakeNext(ref nextOrder, ref sequenceFull),
                    IsActive = item.IsActive,
                    CreatedOn = now,
                    CreatedBy = actor
                },
                cancellationToken);
        }

        await SaveChangesAsync(cancellationToken);

        return await readRepository.ListByRoleIdAsync(dto.RoleId, cancellationToken);
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

    private async Task<FgsRoleMenu?> FindEntityAsync(
        long id,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken) =>
        await context.FgsRoleMenus.FirstOrDefaultIncludingInactiveAsync(
            x => x.Id == id && x.TenantId == tenantId && x.CompanyId == companyId,
            cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "A role menu with the same RoleId and MenuId already exists.",
                ex);
        }
    }

    private void StampForUpdate(FgsRoleMenu entity)
    {
        entity.UpdatedOn = DateTimeOffset.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    private string ResolveActor() =>
        userContext.Email
        ?? userContext.DisplayName
        ?? userContext.UserId?.ToString()
        ?? "system";

    private async Task<FgsRoleMenuDetailDto> ReloadAsync(long id, CancellationToken cancellationToken) =>
        await readRepository.GetByIdAsync(id, cancellationToken)
        ?? throw new InvalidOperationException($"Role menu '{id}' was not found after save.");

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;
}
