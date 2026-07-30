using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Queries.ListTruckStockTemplates;

public sealed record ListTruckStockTemplatesQuery(
    InventoryListQuery Query,
    FgsTruckStockTemplateListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsTruckStockTemplateSummaryDto>>>;
