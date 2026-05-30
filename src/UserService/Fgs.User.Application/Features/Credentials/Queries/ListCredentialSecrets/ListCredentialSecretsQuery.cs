using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Credentials.DTOs;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Queries.ListCredentialSecrets;

public sealed class ListCredentialSecretsQuery : IRequest<ApiResponse<IReadOnlyList<CredentialSecretMetadataDto>>>
{
    public long TenantId { get; init; }

    public long CompanyId { get; init; }

    public Guid? ProviderId { get; init; }

    public bool ActiveOnly { get; init; } = true;
}
