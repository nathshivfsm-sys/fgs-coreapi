using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetStatuses.Queries.GetFgsAssetStatusById;

public sealed record GetFgsAssetStatusByIdQuery(long Id) : IRequest<ApiResponse<FgsAssetStatusDetailDto>>;
