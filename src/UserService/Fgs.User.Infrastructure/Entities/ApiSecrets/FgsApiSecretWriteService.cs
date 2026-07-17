using System.Security.Cryptography;
using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.ApiSecrets;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Features.ApiSecrets.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.ApiSecrets;

public sealed class FgsApiSecretWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext,
    IInvitationTokenService invitationTokenService) : IFgsApiSecretWriteService
{
    public async Task<FgsApiSecretCreateResultDto> CreateAsync(
        FgsApiSecretCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var now = DateTimeOffset.UtcNow;
        var actor = ResolveActor();

        var clientExists = await context.FgsApiClients.AnyAsync(
            c => c.Id == dto.FgsApiClientId && c.TenantId == tenantId && c.CompanyId == companyId,
            cancellationToken);
        if (!clientExists)
        {
            throw new KeyNotFoundException($"API client '{dto.FgsApiClientId}' was not found.");
        }

        var plaintextSecret = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var secretHash = invitationTokenService.HashToken(plaintextSecret);

        var entity = new FgsApiSecret
        {
            TenantId = tenantId,
            CompanyId = companyId,
            FgsApiClientId = dto.FgsApiClientId,
            Name = dto.Name.Trim(),
            SecretHash = secretHash,
            ExpiresOn = dto.ExpiresOn,
            IsActive = true,
            CreatedOn = now,
            CreatedBy = actor
        };

        await context.FgsApiSecrets.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return new FgsApiSecretCreateResultDto(
            entity.Id,
            entity.FgsApiClientId,
            entity.Name,
            plaintextSecret,
            entity.ExpiresOn,
            entity.IsActive,
            entity.CreatedOn,
            entity.CreatedBy);
    }

    public async Task<FgsApiSecretDetailDto> PatchAsync(
        long id,
        FgsApiSecretPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"API secret '{id}' was not found.");

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.ExpiresOn.HasValue)
        {
            entity.ExpiresOn = dto.ExpiresOn;
        }

        if (dto.IsActive.HasValue)
        {
            if (!dto.IsActive.Value)
            {
                ApplyRevocation(entity);
            }
            else
            {
                entity.IsActive = true;
            }
        }

        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsApiSecretDetailDto> RevokeAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"API secret '{id}' was not found.");

        ApplyRevocation(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsApiSecret?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await context.FgsApiSecrets.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

    private void ApplyRevocation(FgsApiSecret entity)
    {
        entity.IsActive = false;
        entity.RevokedOn = DateTimeOffset.UtcNow;
        entity.RevokedBy = ResolveActor();
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
                "An API secret with this name already exists for the client.",
                ex);
        }
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

    private static FgsApiSecretDetailDto MapToDetail(FgsApiSecret entity) =>
        new(
            entity.Id,
            entity.FgsApiClientId,
            entity.Name,
            entity.ExpiresOn,
            entity.LastUsedOn,
            entity.RevokedOn,
            entity.RevokedBy,
            entity.IsActive,
            entity.CreatedOn,
            entity.CreatedBy);
}
