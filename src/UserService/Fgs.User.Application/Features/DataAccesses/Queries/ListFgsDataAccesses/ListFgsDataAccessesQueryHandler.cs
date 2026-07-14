using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.DataAccesses;
using Fgs.User.Application.Features.DataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccesses.Queries.ListFgsDataAccesses;

public sealed class ListFgsDataAccessesQueryHandler(IFgsDataAccessReadRepository readRepository)
    : IRequestHandler<ListFgsDataAccessesQuery, ApiResponse<PagedResult<FgsDataAccessSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsDataAccessSummaryDto>>> Handle(
        ListFgsDataAccessesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsDataAccessSummaryDto>>.Ok(result);
    }
}
