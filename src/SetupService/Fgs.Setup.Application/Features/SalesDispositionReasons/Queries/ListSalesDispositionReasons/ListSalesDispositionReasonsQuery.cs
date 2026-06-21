using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Queries.ListSalesDispositionReasons;

public sealed record ListSalesDispositionReasonsQuery(
    SetupListQuery Query, FgsSalesDispositionReasonListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSalesDispositionReasonSummaryDto>>>;
