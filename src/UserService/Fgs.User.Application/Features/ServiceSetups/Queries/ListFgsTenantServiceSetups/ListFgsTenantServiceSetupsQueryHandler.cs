using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.ServiceSetups;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceSetups.Queries.ListFgsTenantServiceSetups;

public sealed class ListFgsTenantServiceSetupsQueryHandler(IFgsTenantServiceSetupReadRepository readRepository)
    : IRequestHandler<ListFgsTenantServiceSetupsQuery, ApiResponse<PagedResult<FgsTenantServiceSetupSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsTenantServiceSetupSummaryDto>>> Handle(
        ListFgsTenantServiceSetupsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, cancellationToken);
        return ApiResponse<PagedResult<FgsTenantServiceSetupSummaryDto>>.Ok(result);
    }
}
