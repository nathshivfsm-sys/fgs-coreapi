using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetModels.Commands.PatchFgsAssetModel;

public sealed record PatchFgsAssetModelCommand(long Id, FgsAssetModelPatchDto Dto)
    : IRequest<ApiResponse<FgsAssetModelDetailDto>>;
