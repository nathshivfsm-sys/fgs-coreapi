using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetTypes.Commands.CreateFgsAssetType;

public sealed record CreateFgsAssetTypeCommand(FgsAssetTypeCreateDto Dto)
    : IRequest<ApiResponse<FgsAssetTypeDetailDto>>;
