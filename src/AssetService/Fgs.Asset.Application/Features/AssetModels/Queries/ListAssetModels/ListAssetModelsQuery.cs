using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetModels.Queries.ListAssetModels;

public sealed record ListAssetModelsQuery(AssetListQuery Query, FgsAssetModelListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsAssetModelSummaryDto>>>;
