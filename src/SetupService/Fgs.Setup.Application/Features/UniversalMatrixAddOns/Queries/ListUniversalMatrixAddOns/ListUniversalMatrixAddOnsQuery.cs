using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Queries.ListUniversalMatrixAddOns;

public sealed record ListUniversalMatrixAddOnsQuery(
    SetupListQuery Query, FgsUniversalMatrixAddOnListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsUniversalMatrixAddOnSummaryDto>>>;
