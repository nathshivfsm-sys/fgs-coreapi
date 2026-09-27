using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypes.Queries.GetJobTypeCounts;

public sealed record GetJobTypeCountsQuery(
    string? Search,
    JobTypeListFilters Filters)
    : IRequest<ApiResponse<JobTypeCountsDto>>;
