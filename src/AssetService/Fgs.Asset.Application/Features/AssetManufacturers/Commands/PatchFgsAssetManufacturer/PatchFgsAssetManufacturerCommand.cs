using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetManufacturers.Commands.PatchFgsAssetManufacturer;

public sealed record PatchFgsAssetManufacturerCommand(long Id, FgsAssetManufacturerPatchDto Dto)
    : IRequest<ApiResponse<FgsAssetManufacturerDetailDto>>;
