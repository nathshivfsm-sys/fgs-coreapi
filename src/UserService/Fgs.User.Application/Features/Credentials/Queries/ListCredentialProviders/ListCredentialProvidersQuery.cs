using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Credentials.DTOs;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Queries.ListCredentialProviders;

public sealed class ListCredentialProvidersQuery : IRequest<ApiResponse<IReadOnlyList<CredentialProviderMetadataDto>>>
{
    public long TenantId { get; init; }

    public long CompanyId { get; init; }
}
