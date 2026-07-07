using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Queries.ListUniversalMatrixOneTimeFees;

public sealed record ListUniversalMatrixOneTimeFeesQuery(
    SetupListQuery Query, FgsUniversalMatrixOneTimeFeeListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsUniversalMatrixOneTimeFeeSummaryDto>>>;
