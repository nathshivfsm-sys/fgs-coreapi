using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.Invitations;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Features.UserRoles.Dtos;
using Fgs.User.Application.Features.Users.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.Users;

public sealed class FgsUserWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext,
    IFgsUserReadRepository readRepository,
    IFgsRoleReadRepository roleReadRepository,
    IFgsUserRoleWriteService userRoleWriteService,
    IUserInvitationIssuer invitationIssuer) : IFgsUserWriteService
{
    public async Task<FgsUserDetailDto> InviteAsync(
        FgsUserInviteDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        _ = await roleReadRepository.GetByIdAsync(dto.RoleId, cancellationToken)
            ?? throw new KeyNotFoundException($"Role '{dto.RoleId}' was not found.");

        var now = DateTimeOffset.UtcNow;
        var actor = ResolveActor();
        var userId = Guid.NewGuid();

        var entity = new FgsUser
        {
            Id = userId,
            TenantId = tenantId,
            CompanyId = companyId,
            Email = dto.Email.Trim(),
            DisplayName = dto.DisplayName.Trim(),
            AuthenticationMethod = AuthenticationMethod.PasswordOrEmailOtp,
            IsActive = true,
            CreatedOn = now,
            CreatedBy = actor
        };

        await context.FgsUsers.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await userRoleWriteService.CreateAsync(new FgsUserRoleCreateDto(userId, dto.RoleId), cancellationToken);

        await invitationIssuer.IssueAsync(
            new IssueInvitationRequest(
                userId,
                tenantId,
                companyId,
                entity.Email,
                entity.DisplayName,
                InvitationEmailKind.UserInvited,
                CreatedBy: actor,
                UtcNow: now,
                CompanyName: await ResolveCompanyNameAsync(companyId, cancellationToken)),
            cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return (await readRepository.GetByIdAsync(userId, cancellationToken))!;
    }

    public async Task<FgsUserDetailDto> UpdateAsync(
        Guid id,
        FgsUserUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"User '{id}' was not found.");

        entity.DisplayName = dto.DisplayName.Trim();
        entity.IsActive = dto.IsActive;
        StampForUpdate(entity);

        await ReplaceRoleAsync(entity.Id, dto.RoleId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return (await readRepository.GetByIdAsync(entity.Id, cancellationToken))!;
    }

    public async Task<FgsUserDetailDto> PatchAsync(
        Guid id,
        FgsUserPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"User '{id}' was not found.");

        if (dto.DisplayName is not null)
        {
            entity.DisplayName = dto.DisplayName.Trim();
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        StampForUpdate(entity);

        if (dto.RoleId.HasValue)
        {
            await ReplaceRoleAsync(entity.Id, dto.RoleId.Value, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return (await readRepository.GetByIdAsync(entity.Id, cancellationToken))!;
    }

    public async Task<FgsUserDetailDto> ResendInviteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"User '{id}' was not found.");

        var accepted = await context.FgsInvitations.AnyAsync(
            i => i.UserId == id && i.Status == InvitationStatus.Accepted,
            cancellationToken);
        if (accepted)
        {
            throw new InvalidOperationException("Cannot resend invite for a user who has already accepted.");
        }

        var now = DateTimeOffset.UtcNow;
        await invitationIssuer.IssueAsync(
            new IssueInvitationRequest(
                entity.Id,
                tenantId,
                companyId,
                entity.Email,
                entity.DisplayName,
                InvitationEmailKind.UserInvited,
                CreatedBy: ResolveActor(),
                UtcNow: now,
                SupersedePendingForUser: true,
                CompanyName: await ResolveCompanyNameAsync(companyId, cancellationToken)),
            cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return (await readRepository.GetByIdAsync(entity.Id, cancellationToken))!;
    }

    private async Task<string> ResolveCompanyNameAsync(long companyId, CancellationToken cancellationToken)
    {
        var name = await context.FgsTenantCompanyCaches.AsNoTracking()
            .Where(c => c.CompanyId == companyId)
            .Select(c => c.CompanyName)
            .FirstOrDefaultAsync(cancellationToken);
        return name ?? string.Empty;
    }

    private async Task ReplaceRoleAsync(Guid userId, long roleId, CancellationToken cancellationToken)
    {
        _ = await roleReadRepository.GetByIdAsync(roleId, cancellationToken)
            ?? throw new KeyNotFoundException($"Role '{roleId}' was not found.");

        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var existing = await context.FgsUserRoles
            .Where(ur => ur.UserId == userId && ur.TenantId == tenantId && ur.CompanyId == companyId)
            .ToListAsync(cancellationToken);

        if (existing.Count == 1 && existing[0].FgsRoleId == roleId)
        {
            return;
        }

        foreach (var assignment in existing)
        {
            await userRoleWriteService.DeleteAsync(assignment.Id, cancellationToken);
        }

        await userRoleWriteService.CreateAsync(new FgsUserRoleCreateDto(userId, roleId), cancellationToken);
    }

    private async Task<FgsUser?> FindEntityAsync(Guid id, CancellationToken cancellationToken)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        return await context.FgsUsers.FirstOrDefaultAsync(
            u => u.Id == id && u.TenantId == tenantId && u.CompanyId == companyId && !u.IsDeleted,
            cancellationToken);
    }

    private void StampForUpdate(FgsUser entity)
    {
        entity.UpdatedOn = DateTimeOffset.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    private string ResolveActor() =>
        userContext.Email
        ?? userContext.DisplayName
        ?? userContext.UserId?.ToString()
        ?? "system";
}
