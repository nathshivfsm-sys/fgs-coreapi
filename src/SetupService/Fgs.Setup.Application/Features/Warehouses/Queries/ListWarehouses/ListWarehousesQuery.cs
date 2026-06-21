using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Warehouses.Queries.ListWarehouses;

public sealed record ListWarehousesQuery(
    SetupListQuery Query, FgsWarehouseListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsWarehouseSummaryDto>>>;
