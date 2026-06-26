using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Queries.ListSalesPipelineStatuses;

public sealed record ListSalesPipelineStatusesQuery(
    SetupListQuery Query, FgsSalesPipelineStatusListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSalesPipelineStatusSummaryDto>>>;
