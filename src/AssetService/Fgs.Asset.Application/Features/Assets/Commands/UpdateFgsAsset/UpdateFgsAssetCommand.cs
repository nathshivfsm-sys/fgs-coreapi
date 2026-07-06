using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.Assets.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.Assets.Commands.UpdateFgsAsset;

public sealed record UpdateFgsAssetCommand(long Id, FgsAssetUpdateDto Dto)
    : IRequest<ApiResponse<FgsAssetDetailDto>>;
