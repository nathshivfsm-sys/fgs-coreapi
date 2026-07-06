using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetStatuses.Queries.ListAssetStatuses;

public sealed record ListAssetStatusesQuery(AssetListQuery Query, FgsAssetStatusListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsAssetStatusSummaryDto>>>;
