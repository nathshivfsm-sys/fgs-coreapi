using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.PublicEndpoints;
using Fgs.User.Application.Features.PublicEndpoints;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.PublicEndpoints;

public sealed class FgsPublicEndpointWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext) : IFgsPublicEndpointWriteService
{
    public async Task<FgsPublicEndpointDetailDto> CreateAsync(
        FgsPublicEndpointCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var now = DateTimeOffset.UtcNow;
        var actor = ResolveActor();

        var entity = new FgsPublicEndpoint
        {
            TenantId = tenantId,
            CompanyId = companyId,
            EndpointType = PublicEndpointCodes.Normalize(dto.EndpointType),
            EnvironmentCode = PublicEndpointCodes.Normalize(dto.EnvironmentCode),
            BaseUrl = dto.BaseUrl.Trim(),
            DisplayName = dto.DisplayName?.Trim(),
            IsActive = true,
            CreatedOn = now,
            CreatedBy = actor
        };

        await context.FgsPublicEndpoints.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsPublicEndpointDetailDto> UpdateAsync(
        long id,
        FgsPublicEndpointUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Public endpoint '{id}' was not found.");

        entity.EndpointType = PublicEndpointCodes.Normalize(dto.EndpointType);
        entity.EnvironmentCode = PublicEndpointCodes.Normalize(dto.EnvironmentCode);
        entity.BaseUrl = dto.BaseUrl.Trim();
        entity.DisplayName = dto.DisplayName?.Trim();
        StampForUpdate(entity);

        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsPublicEndpointDetailDto> PatchAsync(
        long id,
        FgsPublicEndpointPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Public endpoint '{id}' was not found.");

        if (dto.EndpointType is not null)
        {
            entity.EndpointType = PublicEndpointCodes.Normalize(dto.EndpointType);
        }

        if (dto.EnvironmentCode is not null)
        {
            entity.EnvironmentCode = PublicEndpointCodes.Normalize(dto.EnvironmentCode);
        }

        if (dto.BaseUrl is not null)
        {
            entity.BaseUrl = dto.BaseUrl.Trim();
        }

        if (dto.DisplayName is not null)
        {
            entity.DisplayName = dto.DisplayName.Trim();
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsPublicEndpoint?> FindEntityAsync(long id, CancellationToken cancellationToken)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        return await context.FgsPublicEndpoints.FirstOrDefaultAsync(
            item => item.Id == id && item.TenantId == tenantId && item.CompanyId == companyId,
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
                "A public endpoint with the same type and environment already exists.",
                ex);
        }
    }

    private void StampForUpdate(FgsPublicEndpoint entity)
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

    private static FgsPublicEndpointDetailDto MapToDetail(FgsPublicEndpoint entity) =>
        new(
            entity.Id,
            entity.EndpointType,
            entity.EnvironmentCode,
            entity.BaseUrl,
            entity.DisplayName,
            entity.IsActive);
}
