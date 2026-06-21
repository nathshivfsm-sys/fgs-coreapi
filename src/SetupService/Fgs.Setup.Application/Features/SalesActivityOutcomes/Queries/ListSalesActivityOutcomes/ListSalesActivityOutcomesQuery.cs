using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Queries.ListSalesActivityOutcomes;

public sealed record ListSalesActivityOutcomesQuery(
    SetupListQuery Query, FgsSalesActivityOutcomeListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSalesActivityOutcomeSummaryDto>>>;
