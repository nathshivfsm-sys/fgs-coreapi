using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetTypes.Queries.GetFgsAssetTypeById;

public sealed record GetFgsAssetTypeByIdQuery(long Id) : IRequest<ApiResponse<FgsAssetTypeDetailDto>>;
