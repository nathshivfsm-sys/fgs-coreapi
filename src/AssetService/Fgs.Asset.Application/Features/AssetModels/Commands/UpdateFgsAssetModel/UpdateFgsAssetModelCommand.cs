using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetModels.Commands.UpdateFgsAssetModel;

public sealed record UpdateFgsAssetModelCommand(long Id, FgsAssetModelUpdateDto Dto)
    : IRequest<ApiResponse<FgsAssetModelDetailDto>>;
