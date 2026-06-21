using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Queries.ListActiveSalesPipelineStatuses;

public sealed record ListActiveSalesPipelineStatusesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsSalesPipelineStatusListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsSalesPipelineStatusSummaryDto>>>;
