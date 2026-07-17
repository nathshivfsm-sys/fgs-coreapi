using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.ApiWebhooks;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.ApiWebhooks;

public sealed class FgsApiWebhookWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext) : IFgsApiWebhookWriteService
{
    public async Task<FgsApiWebhookDetailDto> CreateAsync(
        FgsApiWebhookCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var now = DateTimeOffset.UtcNow;
        var actor = ResolveActor();

        var entity = new FgsApiWebhook
        {
            TenantId = tenantId,
            CompanyId = companyId,
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim(),
            EndpointUrl = dto.EndpointUrl.Trim(),
            AuthenticationType = dto.AuthenticationType.Trim(),
            AuthenticationValue = dto.AuthenticationValue?.Trim(),
            Secret = dto.Secret?.Trim(),
            TimeoutSeconds = dto.TimeoutSeconds,
            MaximumRetryCount = dto.MaximumRetryCount,
            IsActive = true,
            CreatedOn = now,
            CreatedBy = actor
        };

        await context.FgsApiWebhooks.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsApiWebhookDetailDto> UpdateAsync(
        long id,
        FgsApiWebhookUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"API webhook '{id}' was not found.");

        entity.Name = dto.Name.Trim();
        entity.Description = dto.Description?.Trim();
        entity.EndpointUrl = dto.EndpointUrl.Trim();
        entity.AuthenticationType = dto.AuthenticationType.Trim();
        entity.AuthenticationValue = dto.AuthenticationValue?.Trim();
        entity.Secret = dto.Secret?.Trim();
        entity.TimeoutSeconds = dto.TimeoutSeconds;
        entity.MaximumRetryCount = dto.MaximumRetryCount;
        StampForUpdate(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsApiWebhookDetailDto> PatchAsync(
        long id,
        FgsApiWebhookPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"API webhook '{id}' was not found.");

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.Description is not null)
        {
            entity.Description = dto.Description.Trim();
        }

        if (dto.EndpointUrl is not null)
        {
            entity.EndpointUrl = dto.EndpointUrl.Trim();
        }

        if (dto.AuthenticationType is not null)
        {
            entity.AuthenticationType = dto.AuthenticationType.Trim();
        }

        if (dto.AuthenticationValue is not null)
        {
            entity.AuthenticationValue = dto.AuthenticationValue.Trim();
        }

        if (dto.Secret is not null)
        {
            entity.Secret = dto.Secret.Trim();
        }

        if (dto.TimeoutSeconds.HasValue)
        {
            entity.TimeoutSeconds = dto.TimeoutSeconds.Value;
        }

        if (dto.MaximumRetryCount.HasValue)
        {
            entity.MaximumRetryCount = dto.MaximumRetryCount.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        StampForUpdate(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsApiWebhook?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await context.FgsApiWebhooks.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

    private void StampForUpdate(FgsApiWebhook entity)
    {
        entity.UpdatedOn = DateTimeOffset.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    private string? ResolveActor() =>
        userContext.Email
        ?? userContext.DisplayName
        ?? userContext.UserId?.ToString();

    private static FgsApiWebhookDetailDto MapToDetail(FgsApiWebhook entity) =>
        new(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.EndpointUrl,
            entity.AuthenticationType,
            entity.AuthenticationValue,
            entity.Secret,
            entity.TimeoutSeconds,
            entity.MaximumRetryCount,
            entity.LastSuccessfulDeliveryOn,
            entity.IsActive);
}
