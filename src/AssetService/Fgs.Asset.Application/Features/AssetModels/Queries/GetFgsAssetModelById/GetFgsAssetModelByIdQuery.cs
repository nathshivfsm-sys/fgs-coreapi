using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetModels.Queries.GetFgsAssetModelById;

public sealed record GetFgsAssetModelByIdQuery(long Id) : IRequest<ApiResponse<FgsAssetModelDetailDto>>;
