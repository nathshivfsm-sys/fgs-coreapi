using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.ApiSecrets;
using Fgs.User.Application.Features.ApiSecrets.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiSecrets.Queries.ListFgsApiSecrets;

public sealed class ListFgsApiSecretsQueryHandler(IFgsApiSecretReadRepository readRepository)
    : IRequestHandler<ListFgsApiSecretsQuery, ApiResponse<PagedResult<FgsApiSecretSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsApiSecretSummaryDto>>> Handle(
        ListFgsApiSecretsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsApiSecretSummaryDto>>.Ok(result);
    }
}
