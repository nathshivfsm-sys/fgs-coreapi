using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetManufacturers.Queries.ListAssetManufacturers;

public sealed record ListAssetManufacturersQuery(AssetListQuery Query, FgsAssetManufacturerListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsAssetManufacturerSummaryDto>>>;
