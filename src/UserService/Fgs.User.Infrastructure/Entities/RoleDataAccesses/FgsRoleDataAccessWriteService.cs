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

        var roleExists = await context.FgsRoles.AnyAsync(
            r => r.Id == dto.FgsRoleId && r.TenantId == tenantId && r.CompanyId == companyId,
            cancellationToken);
        if (!roleExists)
        {
            throw new KeyNotFoundException($"Role '{dto.FgsRoleId}' was not found.");
        }

        var dataAccessExists = await context.FgsDataAccesses.AnyAsync(
            d => d.Id == dto.FgsDataAccessId && d.TenantId == tenantId && d.CompanyId == companyId,
            cancellationToken);
        if (!dataAccessExists)
        {
            throw new KeyNotFoundException($"Data access '{dto.FgsDataAccessId}' was not found.");
        }

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
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("This data access profile is already assigned to the role.", ex);
        }

        return new FgsRoleDataAccessDetailDto(
            entity.Id,
            entity.FgsRoleId,
            entity.FgsDataAccessId,
            entity.CreatedOn,
            entity.CreatedBy);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var entity = await context.FgsRoleDataAccesses.FirstOrDefaultAsync(
            x => x.Id == id && x.TenantId == tenantId && x.CompanyId == companyId,
            cancellationToken)
            ?? throw new KeyNotFoundException($"Role data access assignment '{id}' was not found.");

        context.FgsRoleDataAccesses.Remove(entity);
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
