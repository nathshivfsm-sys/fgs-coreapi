using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeValues.Queries.GetFgsAssetAttributeValueById;

public sealed record GetFgsAssetAttributeValueByIdQuery(long Id) : IRequest<ApiResponse<FgsAssetAttributeValueDetailDto>>;
