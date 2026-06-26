using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Queries.ListSalesActivityTypes;

public sealed record ListSalesActivityTypesQuery(
    SetupListQuery Query, FgsSalesActivityTypeListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSalesActivityTypeSummaryDto>>>;
