using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Sources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Sources.Queries.ListSources;

public sealed record ListSourcesQuery(
    SetupListQuery Query, SourceListFilters Filters)
    : IRequest<ApiResponse<PagedResult<SourcesummaryDto>>>;
