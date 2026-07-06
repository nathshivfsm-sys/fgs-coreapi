using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Queries.ListAssetAttributeOptions;

public sealed record ListAssetAttributeOptionsQuery(AssetListQuery Query, FgsAssetAttributeOptionListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsAssetAttributeOptionSummaryDto>>>;
