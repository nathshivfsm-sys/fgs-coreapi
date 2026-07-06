using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetTypes.Commands.PatchFgsAssetType;

public sealed record PatchFgsAssetTypeCommand(long Id, FgsAssetTypePatchDto Dto)
    : IRequest<ApiResponse<FgsAssetTypeDetailDto>>;
