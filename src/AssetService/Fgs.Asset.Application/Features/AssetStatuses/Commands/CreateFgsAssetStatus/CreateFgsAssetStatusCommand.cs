using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetStatuses.Commands.CreateFgsAssetStatus;

public sealed record CreateFgsAssetStatusCommand(FgsAssetStatusCreateDto Dto)
    : IRequest<ApiResponse<FgsAssetStatusDetailDto>>;
