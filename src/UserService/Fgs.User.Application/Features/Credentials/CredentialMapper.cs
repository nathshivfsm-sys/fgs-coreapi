using System.Text.Json;
using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;

namespace Fgs.User.Application.Features.Credentials;

public static class CredentialKeyBuilder
{
    public static string BuildConfigurationPrefix(CredentialScope scope, string providerCode, long? tenantId, long? companyId) =>
        scope switch
        {
            CredentialScope.Global => $"Global:{providerCode}",
            CredentialScope.Tenant => $"Tenant:{tenantId}:{companyId}:{providerCode}",
            _ => throw new ArgumentOutOfRangeException(nameof(scope), scope, null)
        };

    public static void FlattenJsonIntoDictionary(
        string prefix,
        string json,
        IDictionary<string, string> target)
    {
        using var document = JsonDocument.Parse(json);
        if (document.RootElement.ValueKind != JsonValueKind.Object)
        {
            target[prefix] = json;
            return;
        }

        foreach (var property in document.RootElement.EnumerateObject())
        {
            target[$"{prefix}:{property.Name}"] = property.Value.ValueKind switch
            {
                JsonValueKind.String => property.Value.GetString() ?? string.Empty,
                JsonValueKind.Number => property.Value.GetRawText(),
                JsonValueKind.True => bool.TrueString,
                JsonValueKind.False => bool.FalseString,
                JsonValueKind.Null => string.Empty,
                _ => property.Value.GetRawText()
            };
        }
    }

    public static string FormatGlobalId(int id) => id.ToString();

    public static string FormatTenantId(Guid id) => id.ToString("D");
}

internal static class CredentialMapper
{
    public static CredentialSummaryDto ToSummary(GloCredential credential) =>
        new(
            CredentialScope.Global,
            credential.Id.ToString(),
            credential.ProviderType.ProviderCode,
            credential.ProviderType.ProviderName,
            credential.CredentialName,
            credential.Description,
            credential.IsActive,
            credential.KeyIdentifier,
            null,
            null,
            credential.CreatedOn,
            credential.UpdatedOn);

    public static CredentialSummaryDto ToSummary(FgsCredential credential) =>
        new(
            CredentialScope.Tenant,
            credential.Id.ToString("D"),
            credential.ProviderType.ProviderCode,
            credential.ProviderType.ProviderName,
            credential.CredentialName,
            credential.Description,
            credential.IsActive,
            credential.KeyIdentifier,
            credential.TenantId,
            credential.CompanyId,
            credential.CreatedOn,
            credential.UpdatedOn);

    public static CredentialDetailDto ToDetail(GloCredential credential) =>
        new(
            CredentialScope.Global,
            credential.Id.ToString(),
            credential.ProviderType.ProviderCode,
            credential.ProviderType.ProviderName,
            credential.CredentialName,
            credential.Description,
            credential.IsActive,
            credential.KeyIdentifier,
            null,
            null,
            credential.CreatedOn,
            credential.CreatedBy,
            credential.UpdatedOn,
            credential.UpdatedBy);

    public static CredentialDetailDto ToDetail(FgsCredential credential) =>
        new(
            CredentialScope.Tenant,
            credential.Id.ToString("D"),
            credential.ProviderType.ProviderCode,
            credential.ProviderType.ProviderName,
            credential.CredentialName,
            credential.Description,
            credential.IsActive,
            credential.KeyIdentifier,
            credential.TenantId,
            credential.CompanyId,
            credential.CreatedOn,
            credential.CreatedBy,
            credential.UpdatedOn,
            credential.UpdatedBy);
}
