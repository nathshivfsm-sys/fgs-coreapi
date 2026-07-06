using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetManufacturers.Queries.GetFgsAssetManufacturerById;

public sealed record GetFgsAssetManufacturerByIdQuery(long Id) : IRequest<ApiResponse<FgsAssetManufacturerDetailDto>>;
