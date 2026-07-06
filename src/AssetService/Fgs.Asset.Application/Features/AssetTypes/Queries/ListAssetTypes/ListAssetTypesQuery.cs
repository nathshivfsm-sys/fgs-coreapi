using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetTypes.Queries.ListAssetTypes;

public sealed record ListAssetTypesQuery(AssetListQuery Query, FgsAssetTypeListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsAssetTypeSummaryDto>>>;
