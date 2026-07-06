using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetManufacturers.Commands.CreateFgsAssetManufacturer;

public sealed record CreateFgsAssetManufacturerCommand(FgsAssetManufacturerCreateDto Dto)
    : IRequest<ApiResponse<FgsAssetManufacturerDetailDto>>;
