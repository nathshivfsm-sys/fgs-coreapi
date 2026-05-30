using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials.DTOs;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Queries.GetCredentialMetadata;

public sealed class GetCredentialMetadataQuery : IRequest<ApiResponse<CredentialSecretMetadataDto>>
{
    public Guid SecretId { get; init; }

    public long TenantId { get; init; }

    public long CompanyId { get; init; }
}
