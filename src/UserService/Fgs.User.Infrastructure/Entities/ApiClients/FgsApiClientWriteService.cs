using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.ApiClients;
using Fgs.User.Application.Features.ApiClients.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.ApiClients;

public sealed class FgsApiClientWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext) : IFgsApiClientWriteService
{
    public async Task<FgsApiClientDetailDto> CreateAsync(
        FgsApiClientCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var now = DateTimeOffset.UtcNow;
        var actor = ResolveActor();

        var entity = new FgsApiClient
        {
            TenantId = tenantId,
            CompanyId = companyId,
            ClientId = Guid.NewGuid(),
            ApplicationName = dto.ApplicationName.Trim(),
            Description = dto.Description?.Trim(),
            ContactName = dto.ContactName?.Trim(),
            ContactEmail = dto.ContactEmail?.Trim(),
            RateLimitPerMinute = dto.RateLimitPerMinute,
            IsActive = true,
            CreatedOn = now,
            CreatedBy = actor
        };

        await context.FgsApiClients.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsApiClientDetailDto> UpdateAsync(
        long id,
        FgsApiClientUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"API client '{id}' was not found.");

        entity.ApplicationName = dto.ApplicationName.Trim();
        entity.Description = dto.Description?.Trim();
        entity.ContactName = dto.ContactName?.Trim();
        entity.ContactEmail = dto.ContactEmail?.Trim();
        entity.RateLimitPerMinute = dto.RateLimitPerMinute;
        StampForUpdate(entity);

        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsApiClientDetailDto> PatchAsync(
        long id,
        FgsApiClientPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"API client '{id}' was not found.");

        if (dto.ApplicationName is not null)
        {
            entity.ApplicationName = dto.ApplicationName.Trim();
        }

        if (dto.Description is not null)
        {
            entity.Description = dto.Description.Trim();
        }

        if (dto.ContactName is not null)
        {
            entity.ContactName = dto.ContactName.Trim();
        }

        if (dto.ContactEmail is not null)
        {
            entity.ContactEmail = dto.ContactEmail.Trim();
        }

        if (dto.RateLimitPerMinute.HasValue)
        {
            entity.RateLimitPerMinute = dto.RateLimitPerMinute.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsApiClient?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await context.FgsApiClients.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "An API client with the same application name already exists.",
                ex);
        }
    }

    private void StampForUpdate(FgsApiClient entity)
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

    private static FgsApiClientDetailDto MapToDetail(FgsApiClient entity) =>
        new(
            entity.Id,
            entity.ClientId,
            entity.ApplicationName,
            entity.Description,
            entity.ContactName,
            entity.ContactEmail,
            entity.RateLimitPerMinute,
            entity.IsActive);
}
