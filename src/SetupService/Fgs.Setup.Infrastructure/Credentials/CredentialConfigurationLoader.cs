using System.Text;
using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Application.Features.Credentials;
using Fgs.Setup.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Infrastructure.Credentials;

public sealed class CredentialConfigurationLoader
{
    private readonly ICredentialRepository _repository;
    private readonly ICredentialEncryptionService _encryptionService;
    private readonly CredentialConfigurationHolder _holder;
    private readonly ILogger<CredentialConfigurationLoader> _logger;

    public CredentialConfigurationLoader(
        ICredentialRepository repository,
        ICredentialEncryptionService encryptionService,
        CredentialConfigurationHolder holder,
        ILogger<CredentialConfigurationLoader> logger)
    {
        _repository = repository;
        _encryptionService = encryptionService;
        _holder = holder;
        _logger = logger;
    }

    public async Task ReloadAsync(CancellationToken cancellationToken = default)
    {
        var flattened = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        var globalCredentials = await _repository.ListGlobalAsync(activeOnly: true, cancellationToken);
        foreach (var credential in globalCredentials)
        {
            await AddCredentialAsync(
                CredentialScope.Global,
                credential.ProviderType.ProviderCode,
                null,
                null,
                credential.CredentialData,
                credential.EncryptedDataKey,
                flattened,
                cancellationToken);
        }

        var tenantCredentials = await _repository.ListTenantAsync(null, null, activeOnly: true, cancellationToken);
        foreach (var credential in tenantCredentials)
        {
            await AddCredentialAsync(
                CredentialScope.Tenant,
                credential.ProviderType.ProviderCode,
                credential.TenantId,
                credential.CompanyId,
                credential.CredentialData,
                credential.EncryptedDataKey,
                flattened,
                cancellationToken);
        }

        _holder.ReplaceValues(flattened);
        _logger.LogInformation("Loaded {Count} resolved credential configuration entries.", flattened.Count);
    }

    private async Task AddCredentialAsync(
        CredentialScope scope,
        string providerCode,
        long? tenantId,
        long? companyId,
        byte[] credentialData,
        byte[] encryptedDataKey,
        IDictionary<string, string> target,
        CancellationToken cancellationToken)
    {
        try
        {
            var plaintext = await _encryptionService.DecryptAsync(credentialData, encryptedDataKey, cancellationToken);
            var json = Encoding.UTF8.GetString(plaintext);
            var prefix = CredentialKeyBuilder.BuildConfigurationPrefix(scope, providerCode, tenantId, companyId);
            CredentialKeyBuilder.FlattenJsonIntoDictionary(prefix, json, target);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Failed to decrypt credential for provider {ProviderCode} ({Scope}).",
                providerCode,
                scope);
        }
    }
}
