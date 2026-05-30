using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials.DTOs;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Queries.GetCredentialSecretForTest;

/// <summary>
/// Resolves decrypted secret values for local/testing only. Must not be mapped in production controllers.
/// </summary>
public sealed class GetCredentialSecretForTestQuery : IRequest<ApiResponse<CredentialSecretTestDto>>
{
    public Guid SecretId { get; init; }

    public long TenantId { get; init; }

    public long CompanyId { get; init; }

    public string? AccessedBy { get; init; }
}
