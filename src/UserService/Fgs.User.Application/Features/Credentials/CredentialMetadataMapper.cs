using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Domain.Entities;

namespace Fgs.User.Application.Features.Credentials;

public static class CredentialMetadataMapper
{
    public static CredentialSecretMetadataDto ToSecretMetadata(
        FgsCredentialSecret secret,
        FgsCredentialProvider provider,
        string? providerTypeCode) =>
        new()
        {
            SecretId = secret.Id,
            ProviderId = provider.Id,
            ProviderCode = provider.Code,
            ProviderName = provider.Name,
            SecretName = secret.SecretName,
            AwsSecretArn = CredentialSecretStorageMapping.GetAwsSecretArn(secret),
            RegionName = CredentialSecretStorageMapping.GetRegionName(secret),
            VersionNo = secret.VersionNo,
            IsActive = secret.IsActive,
            IsRevoked = secret.IsRevoked,
            LastRotatedOn = secret.LastRotatedOn,
            ExpiresOn = secret.ExpiresOn,
            CreatedOn = secret.CreatedOn,
            UpdatedOn = secret.UpdatedOn
        };

    public static CredentialProviderMetadataDto ToProviderMetadata(
        FgsCredentialProvider provider,
        string? providerTypeCode) =>
        new()
        {
            ProviderId = provider.Id,
            Code = provider.Code,
            Name = provider.Name,
            Environment = provider.Environment,
            ProviderTypeId = provider.CredentialProviderTypeId,
            ProviderTypeCode = providerTypeCode,
            IsActive = provider.IsActive
        };
}
