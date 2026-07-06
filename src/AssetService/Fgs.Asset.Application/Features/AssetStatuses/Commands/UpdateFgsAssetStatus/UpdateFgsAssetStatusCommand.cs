using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetStatuses.Commands.UpdateFgsAssetStatus;

public sealed record UpdateFgsAssetStatusCommand(long Id, FgsAssetStatusUpdateDto Dto)
    : IRequest<ApiResponse<FgsAssetStatusDetailDto>>;
