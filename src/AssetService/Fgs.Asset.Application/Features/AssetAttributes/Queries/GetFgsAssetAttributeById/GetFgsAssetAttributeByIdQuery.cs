using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributes.Queries.GetFgsAssetAttributeById;

public sealed record GetFgsAssetAttributeByIdQuery(long Id) : IRequest<ApiResponse<FgsAssetAttributeDetailDto>>;
