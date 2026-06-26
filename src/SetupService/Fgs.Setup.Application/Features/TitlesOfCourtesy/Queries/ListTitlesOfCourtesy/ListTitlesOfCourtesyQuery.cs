using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Queries.ListTitlesOfCourtesy;

public sealed record ListTitlesOfCourtesyQuery(
    SetupListQuery Query,
    TitleOfCourtesyListFilters Filters)
    : IRequest<ApiResponse<PagedResult<TitleOfCourtesySummaryDto>>>;
