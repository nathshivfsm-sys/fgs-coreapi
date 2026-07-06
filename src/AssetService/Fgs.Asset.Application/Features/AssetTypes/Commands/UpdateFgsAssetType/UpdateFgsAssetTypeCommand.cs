using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetTypes.Commands.UpdateFgsAssetType;

public sealed record UpdateFgsAssetTypeCommand(long Id, FgsAssetTypeUpdateDto Dto)
    : IRequest<ApiResponse<FgsAssetTypeDetailDto>>;
