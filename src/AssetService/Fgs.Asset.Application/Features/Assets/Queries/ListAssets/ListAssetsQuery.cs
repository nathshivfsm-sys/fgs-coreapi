using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.Assets.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.Assets.Queries.ListAssets;

public sealed record ListAssetsQuery(AssetListQuery Query, FgsAssetListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsAssetSummaryDto>>>;
