using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.Assets.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.Assets.Commands.CreateFgsAsset;

public sealed record CreateFgsAssetCommand(FgsAssetCreateDto Dto)
    : IRequest<ApiResponse<FgsAssetDetailDto>>;
