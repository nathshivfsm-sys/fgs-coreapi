using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributes.Queries.ListAssetAttributes;

public sealed record ListAssetAttributesQuery(AssetListQuery Query, FgsAssetAttributeListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsAssetAttributeSummaryDto>>>;
