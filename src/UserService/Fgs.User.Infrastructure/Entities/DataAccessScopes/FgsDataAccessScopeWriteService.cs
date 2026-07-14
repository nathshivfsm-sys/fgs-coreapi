using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.DataAccessScopes;
using Fgs.User.Application.Features.DataAccessScopes.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.DataAccessScopes;

public sealed class FgsDataAccessScopeWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext) : IFgsDataAccessScopeWriteService
{
    public async Task<FgsDataAccessScopeDetailDto> CreateAsync(
        FgsDataAccessScopeCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var now = DateTimeOffset.UtcNow;
        var actor = ResolveActor() ?? "system";

        var parent = await context.FgsDataAccesses.FirstOrDefaultAsync(
            item => item.Id == dto.FgsDataAccessId,
            cancellationToken);

        if (parent is null)
        {
            throw new KeyNotFoundException($"Data access '{dto.FgsDataAccessId}' was not found.");
        }

        var entity = new FgsDataAccessScope
        {
            TenantId = tenantId,
            CompanyId = companyId,
            FgsDataAccessId = dto.FgsDataAccessId,
            ScopeType = dto.ScopeType.Trim(),
            Operator = dto.Operator.Trim(),
            ScopeValue = dto.ScopeValue?.Trim(),
            DisplayOrder = dto.DisplayOrder,
            CreatedOn = now,
            CreatedBy = actor
        };

        await context.FgsDataAccessScopes.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsDataAccessScopeDetailDto> UpdateAsync(
        long id,
        FgsDataAccessScopeUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Data access scope '{id}' was not found.");

        entity.ScopeType = dto.ScopeType.Trim();
        entity.Operator = dto.Operator.Trim();
        entity.ScopeValue = dto.ScopeValue?.Trim();
        entity.DisplayOrder = dto.DisplayOrder;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsDataAccessScopeDetailDto> PatchAsync(
        long id,
        FgsDataAccessScopePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Data access scope '{id}' was not found.");

        if (dto.ScopeType is not null)
        {
            entity.ScopeType = dto.ScopeType.Trim();
        }

        if (dto.Operator is not null)
        {
            entity.Operator = dto.Operator.Trim();
        }

        if (dto.ScopeValue is not null)
        {
            entity.ScopeValue = dto.ScopeValue.Trim();
        }

        if (dto.DisplayOrder.HasValue)
        {
            entity.DisplayOrder = dto.DisplayOrder.Value;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsDataAccessScope?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await context.FgsDataAccessScopes.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

    private string? ResolveActor() =>
        userContext.Email
        ?? userContext.DisplayName
        ?? userContext.UserId?.ToString();

    private static FgsDataAccessScopeDetailDto MapToDetail(FgsDataAccessScope entity) =>
        new(
            entity.Id,
            entity.FgsDataAccessId,
            entity.ScopeType,
            entity.Operator,
            entity.ScopeValue,
            entity.DisplayOrder);
}
