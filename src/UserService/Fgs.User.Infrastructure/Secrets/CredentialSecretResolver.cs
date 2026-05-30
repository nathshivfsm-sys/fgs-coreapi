using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Credentials;
using Fgs.User.Application.Features.Credentials.Models;
using Fgs.User.Application.Features.Credentials.Payloads;
using Fgs.User.Domain.Constants;
using Fgs.User.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Infrastructure.Secrets;

public sealed class CredentialSecretResolver(
    IUnitOfWork unitOfWork,
    ISecretsManagerService secretsManager,
    ISecretCache secretCache,
    ICredentialAuditWriter auditWriter,
    ICredentialPayloadDeserializer payloadDeserializer,
    ICredentialConnectionStringBuilder connectionStringBuilder,
    ICorrelationContext correlationContext,
    ILogger<CredentialSecretResolver> logger) : ICredentialSecretResolver
{
    public Task<CredentialSecretResolution?> ResolveAsync(
        long tenantId,
        long companyId,
        Guid secretId,
        string? accessedBy,
        CancellationToken cancellationToken = default) =>
        ResolveCoreAsync(tenantId, companyId, secretId, accessedBy, cancellationToken);

    public async Task<T?> ResolvePayloadAsync<T>(
        long tenantId,
        long companyId,
        Guid secretId,
        string? accessedBy,
        CancellationToken cancellationToken = default) where T : class
    {
        var resolution = await ResolveCoreAsync(tenantId, companyId, secretId, accessedBy, cancellationToken);
        if (resolution is null)
        {
            return null;
        }

        return payloadDeserializer.Deserialize<T>(resolution.ProviderTypeCode, resolution.SecretJson);
    }

    public async Task<string?> ResolveSqlConnectionStringAsync(
        long tenantId,
        long companyId,
        Guid secretId,
        string? accessedBy,
        CancellationToken cancellationToken = default)
    {
        var payload = await ResolvePayloadAsync<SqlDatabaseSecretPayload>(
            tenantId,
            companyId,
            secretId,
            accessedBy,
            cancellationToken);

        return payload is null ? null : connectionStringBuilder.BuildSqlConnectionString(payload);
    }

    private async Task<CredentialSecretResolution?> ResolveCoreAsync(
        long tenantId,
        long companyId,
        Guid secretId,
        string? accessedBy,
        CancellationToken cancellationToken)
    {
        var secretRepo = unitOfWork.Repository<FgsCredentialSecret>();
        var secret = await secretRepo.GetByIdAsync(secretId, cancellationToken);

        if (secret is null
            || secret.TenantId != tenantId
            || secret.CompanyId != companyId)
        {
            logger.LogWarning(
                "Credential secret access denied: not found or tenant mismatch (SecretId={SecretId}, TenantId={TenantId}, CompanyId={CompanyId})",
                secretId,
                tenantId,
                companyId);
            return null;
        }

        if (secret.IsRevoked || !secret.IsActive)
        {
            await WriteDenyAuditAsync(secret, accessedBy, "Secret is revoked or inactive.", cancellationToken);
            return null;
        }

        var provider = await unitOfWork.Repository<FgsCredentialProvider>()
            .GetByIdAsync(secret.CredentialProviderId, cancellationToken);

        if (provider is null)
        {
            await WriteDenyAuditAsync(secret, accessedBy, "Provider not found.", cancellationToken);
            return null;
        }

        var providerType = await unitOfWork.Repository<GloCredentialProviderType>()
            .FirstOrDefaultAsync(p => p.Id == provider.CredentialProviderTypeId, cancellationToken);

        var providerTypeCode = providerType?.Code ?? "OTHER";
        var cacheKey = MemorySecretCache.BuildCacheKey(tenantId, companyId, secretId, secret.VersionNo);

        if (!secretCache.TryGet(cacheKey, out var secretJson))
        {
            var arn = CredentialSecretStorageMapping.GetAwsSecretArn(secret);
            secretJson = await secretsManager.GetSecretJsonAsync(arn, cancellationToken: cancellationToken);
            secretCache.Set(cacheKey, secretJson);
        }

        await auditWriter.WriteAsync(
            secret.TenantId,
            secret.CompanyId,
            secret.Id,
            CredentialAuditActions.SecretAccessed,
            secret.VersionNo,
            secret.VersionNo,
            CredentialAuditRemarks.Format(correlationContext.GetCorrelationId(), null),
            accessedBy,
            cancellationToken: cancellationToken);

        logger.LogDebug(
            "Resolved credential secret {SecretId} for tenant {TenantId} company {CompanyId}",
            secretId,
            tenantId,
            companyId);

        return new CredentialSecretResolution(
            secretId,
            providerTypeCode,
            secretJson,
            secret.VersionNo);
    }

    private Task WriteDenyAuditAsync(
        FgsCredentialSecret secret,
        string? accessedBy,
        string reason,
        CancellationToken cancellationToken) =>
        auditWriter.WriteAsync(
            secret.TenantId,
            secret.CompanyId,
            secret.Id,
            CredentialAuditActions.SecretAccessDenied,
            null,
            null,
            CredentialAuditRemarks.Format(correlationContext.GetCorrelationId(), reason),
            accessedBy,
            cancellationToken: cancellationToken);
}
