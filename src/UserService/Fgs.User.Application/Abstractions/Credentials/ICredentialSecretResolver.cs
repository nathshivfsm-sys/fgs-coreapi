using Fgs.User.Application.Features.Credentials.Models;

namespace Fgs.User.Application.Abstractions.Credentials;

public interface ICredentialSecretResolver
{
    Task<CredentialSecretResolution?> ResolveAsync(
        long tenantId,
        long companyId,
        Guid secretId,
        string? accessedBy,
        CancellationToken cancellationToken = default);

    Task<T?> ResolvePayloadAsync<T>(
        long tenantId,
        long companyId,
        Guid secretId,
        string? accessedBy,
        CancellationToken cancellationToken = default) where T : class;

    Task<string?> ResolveSqlConnectionStringAsync(
        long tenantId,
        long companyId,
        Guid secretId,
        string? accessedBy,
        CancellationToken cancellationToken = default);
}
