using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Queries.ListUniversalMatrixItems;

public sealed record ListUniversalMatrixItemsQuery(
    SetupListQuery Query, FgsUniversalMatrixItemListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsUniversalMatrixItemSummaryDto>>>;
