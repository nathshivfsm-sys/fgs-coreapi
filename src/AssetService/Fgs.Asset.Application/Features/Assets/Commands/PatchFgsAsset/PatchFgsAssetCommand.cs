using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.Assets.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.Assets.Commands.PatchFgsAsset;

public sealed record PatchFgsAssetCommand(long Id, FgsAssetPatchDto Dto)
    : IRequest<ApiResponse<FgsAssetDetailDto>>;
