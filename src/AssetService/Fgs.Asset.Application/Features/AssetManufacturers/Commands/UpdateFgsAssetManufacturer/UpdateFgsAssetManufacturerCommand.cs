using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetManufacturers.Commands.UpdateFgsAssetManufacturer;

public sealed record UpdateFgsAssetManufacturerCommand(long Id, FgsAssetManufacturerUpdateDto Dto)
    : IRequest<ApiResponse<FgsAssetManufacturerDetailDto>>;
