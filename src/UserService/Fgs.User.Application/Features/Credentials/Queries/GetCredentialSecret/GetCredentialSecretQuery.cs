using Fgs.User.Application.Features.Credentials.Models;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Queries.GetCredentialSecret;

/// <summary>
/// Internal-only query. Never expose via public HTTP API.
/// </summary>
public sealed class GetCredentialSecretQuery : IRequest<CredentialSecretResolution?>
{
    public Guid SecretId { get; init; }

    public long TenantId { get; init; }

    public long CompanyId { get; init; }

    public string? AccessedBy { get; init; }
}
