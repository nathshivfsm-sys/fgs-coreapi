using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Features.Roles.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.Roles;

public sealed class FgsRoleWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsRoleReadRepository readRepository,
    IFgsUserContext userContext) : IFgsRoleWriteService
{
    public async Task<FgsRoleDetailDto> CreateAsync(
        FgsRoleCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var now = DateTimeOffset.UtcNow;
        var actor = ResolveActor();

        if (dto.ParentRoleId.HasValue)
        {
            var parentRole = await context.FgsRoles.FirstOrDefaultAsync(
                role => role.Id == dto.ParentRoleId.Value,
                cancellationToken);

            if (parentRole is null)
            {
                throw new KeyNotFoundException($"Parent role '{dto.ParentRoleId.Value}' was not found.");
            }
        }

        var entity = new FgsRole
        {
            TenantId = tenantId,
            CompanyId = companyId,
            ParentRoleId = dto.ParentRoleId,
            RoleCode = NormalizeRoleCode(dto.RoleCode),
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim(),
            IsBuiltIn = false,
            DisplayOrder = dto.DisplayOrder,
            IsActive = true,
            CreatedOn = now,
            CreatedBy = actor
        };

        await context.FgsRoles.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsRoleDetailDto> UpdateAsync(
        long id,
        FgsRoleUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Role '{id}' was not found.");

        EnsureMutable(entity);

        entity.RoleCode = NormalizeRoleCode(dto.RoleCode);
        entity.Name = dto.Name.Trim();
        entity.Description = dto.Description?.Trim();
        entity.DisplayOrder = dto.DisplayOrder;
        StampForUpdate(entity);

        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsRoleDetailDto> PatchAsync(
        long id,
        FgsRolePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Role '{id}' was not found.");

        EnsureMutable(entity);

        if (dto.RoleCode is not null)
        {
            entity.RoleCode = NormalizeRoleCode(dto.RoleCode);
        }

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.Description is not null)
        {
            entity.Description = dto.Description.Trim();
        }

        if (dto.DisplayOrder.HasValue)
        {
            entity.DisplayOrder = dto.DisplayOrder.Value;
        }

        if (dto.IsActive.HasValue)
        {
            if (!dto.IsActive.Value
                && entity.IsActive
                && await readRepository.HasActiveUserAssignmentsAsync(id, cancellationToken))
            {
                throw new InvalidOperationException(
                    "Cannot deactivate a role that is assigned to one or more users.");
            }

            entity.IsActive = dto.IsActive.Value;
        }

        StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsRole?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await context.FgsRoles.FirstOrDefaultAsync(role => role.Id == id, cancellationToken);

    private static void EnsureMutable(FgsRole entity)
    {
        if (entity.IsBuiltIn)
        {
            throw new InvalidOperationException("Built-in roles cannot be edited or deactivated.");
        }
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A role with the same role code already exists.", ex);
        }
    }

    private void StampForUpdate(FgsRole entity)
    {
        entity.UpdatedOn = DateTimeOffset.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    private string? ResolveActor() =>
        userContext.Email
        ?? userContext.DisplayName
        ?? userContext.UserId?.ToString();

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeRoleCode(string roleCode) => roleCode.Trim().ToUpperInvariant();

    private static FgsRoleDetailDto MapToDetail(FgsRole entity) =>
        new(
            entity.Id,
            entity.RoleCode,
            entity.Name,
            entity.Description,
            entity.ParentRoleId,
            entity.IsBuiltIn,
            entity.DisplayOrder,
            entity.IsActive);
}
