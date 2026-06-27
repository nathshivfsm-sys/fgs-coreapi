using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypes.Queries.ListJobTypes;

public sealed class ListJobTypesQueryHandler(IJobTypeReadRepository readRepository)
    : IRequestHandler<ListJobTypesQuery, ApiResponse<PagedResult<JobTypeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<JobTypeSummaryDto>>> Handle(
        ListJobTypesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<JobTypeSummaryDto>>.Ok(result);
    }
}
