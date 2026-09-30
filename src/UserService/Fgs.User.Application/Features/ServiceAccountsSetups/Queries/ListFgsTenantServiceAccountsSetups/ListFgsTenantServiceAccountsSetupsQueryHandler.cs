using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.ServiceAccountsSetups;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceAccountsSetups.Queries.ListFgsTenantServiceAccountsSetups;

public sealed class ListFgsTenantServiceAccountsSetupsQueryHandler(
    IFgsTenantServiceAccountsSetupReadRepository readRepository)
    : IRequestHandler<ListFgsTenantServiceAccountsSetupsQuery, ApiResponse<PagedResult<FgsTenantServiceAccountsSetupSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsTenantServiceAccountsSetupSummaryDto>>> Handle(
        ListFgsTenantServiceAccountsSetupsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, cancellationToken);
        return ApiResponse<PagedResult<FgsTenantServiceAccountsSetupSummaryDto>>.Ok(result);
    }
}
