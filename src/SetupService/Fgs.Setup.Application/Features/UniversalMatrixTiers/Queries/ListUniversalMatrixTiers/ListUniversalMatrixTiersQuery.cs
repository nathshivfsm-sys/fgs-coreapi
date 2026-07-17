using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixTiers.Queries.ListUniversalMatrixTiers;

public sealed record ListUniversalMatrixTiersQuery(
    SetupListQuery Query, FgsUniversalMatrixTierListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsUniversalMatrixTierSummaryDto>>>;
