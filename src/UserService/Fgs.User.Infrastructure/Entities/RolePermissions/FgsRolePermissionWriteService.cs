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

        var roleExists = await context.FgsRoles.AnyAsync(
            r => r.Id == dto.FgsRoleId && r.TenantId == tenantId && r.CompanyId == companyId,
            cancellationToken);
        if (!roleExists)
        {
            throw new KeyNotFoundException($"Role '{dto.FgsRoleId}' was not found.");
        }

        var permissionExists = await context.FgsPermissions.AnyAsync(
            p => p.Id == dto.FgsPermissionId,
            cancellationToken);
        if (!permissionExists)
        {
            throw new KeyNotFoundException($"Permission '{dto.FgsPermissionId}' was not found.");
        }

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
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("This permission is already assigned to the role.", ex);
        }

        return new FgsRolePermissionDetailDto(
            entity.Id,
            entity.FgsRoleId,
            entity.FgsPermissionId,
            entity.CreatedOn,
            entity.CreatedBy);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var entity = await context.FgsRolePermissions.FirstOrDefaultAsync(
            x => x.Id == id && x.TenantId == tenantId && x.CompanyId == companyId,
            cancellationToken)
            ?? throw new KeyNotFoundException($"Role permission assignment '{id}' was not found.");

        context.FgsRolePermissions.Remove(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
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
}
