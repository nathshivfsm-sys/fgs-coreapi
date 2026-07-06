using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetManufacturers.Queries.LookupAssetManufacturers;

public sealed record LookupAssetManufacturersQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsAssetManufacturerLookupDto>>>;
