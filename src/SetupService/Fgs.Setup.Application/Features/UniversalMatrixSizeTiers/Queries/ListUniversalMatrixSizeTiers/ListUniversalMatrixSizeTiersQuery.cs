using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Queries.ListUniversalMatrixSizeTiers;

public sealed record ListUniversalMatrixSizeTiersQuery(
    SetupListQuery Query, FgsUniversalMatrixSizeTierListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsUniversalMatrixSizeTierSummaryDto>>>;
