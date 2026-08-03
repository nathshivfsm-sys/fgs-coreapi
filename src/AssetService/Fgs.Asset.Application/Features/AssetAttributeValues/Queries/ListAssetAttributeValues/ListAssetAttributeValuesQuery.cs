using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeValues.Queries.ListAssetAttributeValues;

public sealed record ListAssetAttributeValuesQuery(AssetListQuery Query, FgsAssetAttributeValueListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsAssetAttributeValueSummaryDto>>>;
