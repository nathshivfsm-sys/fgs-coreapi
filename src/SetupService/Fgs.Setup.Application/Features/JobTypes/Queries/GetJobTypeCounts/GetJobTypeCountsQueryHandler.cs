using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypes.Queries.GetJobTypeCounts;

public sealed class GetJobTypeCountsQueryHandler(IJobTypeReadRepository readRepository)
    : IRequestHandler<GetJobTypeCountsQuery, ApiResponse<JobTypeCountsDto>>
{
    public async Task<ApiResponse<JobTypeCountsDto>> Handle(
        GetJobTypeCountsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetCountsAsync(request.Search, request.Filters, cancellationToken);
        return ApiResponse<JobTypeCountsDto>.Ok(result);
    }
}
