using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetWarranties.Queries.ListAssetWarranties;

public sealed record ListAssetWarrantiesQuery(AssetListQuery Query, FgsAssetWarrantyListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsAssetWarrantySummaryDto>>>;
