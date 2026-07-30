using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Queries.ListActiveTruckStockTemplates;

public sealed record ListActiveTruckStockTemplatesQuery(
    int Page = 1,
    int PageSize = 25,
    string? SortBy = null,
    SortDirection SortDirection = SortDirection.Asc,
    string? Search = null,
    FgsTruckStockTemplateListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsTruckStockTemplateSummaryDto>>>;
