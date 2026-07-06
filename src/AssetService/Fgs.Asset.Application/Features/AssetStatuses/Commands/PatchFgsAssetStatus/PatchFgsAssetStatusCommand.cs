using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetStatuses.Commands.PatchFgsAssetStatus;

public sealed record PatchFgsAssetStatusCommand(long Id, FgsAssetStatusPatchDto Dto)
    : IRequest<ApiResponse<FgsAssetStatusDetailDto>>;
