using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetModels.Commands.CreateFgsAssetModel;

public sealed record CreateFgsAssetModelCommand(FgsAssetModelCreateDto Dto)
    : IRequest<ApiResponse<FgsAssetModelDetailDto>>;
