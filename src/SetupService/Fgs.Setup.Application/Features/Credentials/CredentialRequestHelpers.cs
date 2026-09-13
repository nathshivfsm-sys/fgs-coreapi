using System.Text;
using Fgs.Contracts.Api;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Domain.Enums;

namespace Fgs.Setup.Application.Features.Credentials;

internal static class CredentialRequestHelpers
{
    public static byte[] ParsePayload(string payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
        {
            throw new InvalidOperationException(CredentialErrorMessages.InvalidPayload);
        }

        return Encoding.UTF8.GetBytes(payload);
    }

    public static bool TryParseGlobalId(string id, out int parsed) => int.TryParse(id, out parsed);

    public static bool TryParseTenantId(string id, out Guid parsed) => Guid.TryParse(id, out parsed);

    public static CredentialMutationResultDto ToMutationResult(CredentialScope scope, string id, string providerCode, string credentialName) =>
        new(scope, id, providerCode, credentialName);

    public static ApiResponse<T>? EnsureTenantScope<T>(
        ITenantContextAccessor tenantContextAccessor,
        FgsCredential credential)
    {
        if (tenantContextAccessor.Current is not { } tenantScope)
        {
            return ApiResponse<T>.Fail(
                [CredentialErrorMessages.TenantContextRequired],
                ApiStatusCodes.BadRequest);
        }

        if (credential.TenantId != tenantScope.TenantId || credential.CompanyId != tenantScope.CompanyId)
        {
            return ApiResponse<T>.Fail(
                [CredentialErrorMessages.TenantScopeMismatch],
                ApiStatusCodes.Forbidden);
        }

        return null;
    }
}
