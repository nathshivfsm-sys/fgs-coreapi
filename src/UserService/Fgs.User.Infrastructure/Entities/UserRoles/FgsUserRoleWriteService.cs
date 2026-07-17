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

        var userExists = await context.FgsUsers.AnyAsync(
            u => u.Id == dto.UserId && u.TenantId == tenantId && u.CompanyId == companyId,
            cancellationToken);
        if (!userExists)
        {
            throw new KeyNotFoundException($"User '{dto.UserId}' was not found.");
        }

        var roleExists = await context.FgsRoles.AnyAsync(
            r => r.Id == dto.FgsRoleId && r.TenantId == tenantId && r.CompanyId == companyId,
            cancellationToken);
        if (!roleExists)
        {
            throw new KeyNotFoundException($"Role '{dto.FgsRoleId}' was not found.");
        }

        var entity = new FgsUserRole
        {
            TenantId = tenantId,
            CompanyId = companyId,
            UserId = dto.UserId,
            FgsRoleId = dto.FgsRoleId,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = ResolveActor()
        };

        await context.FgsUserRoles.AddAsync(entity, cancellationToken);
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("This role is already assigned to the user.", ex);
        }

        return new FgsUserRoleDetailDto(entity.Id, entity.UserId, entity.FgsRoleId, entity.CreatedOn, entity.CreatedBy);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var entity = await context.FgsUserRoles.FirstOrDefaultAsync(
            x => x.Id == id && x.TenantId == tenantId && x.CompanyId == companyId,
            cancellationToken)
            ?? throw new KeyNotFoundException($"User role assignment '{id}' was not found.");

        context.FgsUserRoles.Remove(entity);
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
