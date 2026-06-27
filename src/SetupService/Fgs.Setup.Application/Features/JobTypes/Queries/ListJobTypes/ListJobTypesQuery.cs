using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypes.Queries.ListJobTypes;

public sealed record ListJobTypesQuery(
    SetupListQuery Query, JobTypeListFilters Filters)
    : IRequest<ApiResponse<PagedResult<JobTypeSummaryDto>>>;
