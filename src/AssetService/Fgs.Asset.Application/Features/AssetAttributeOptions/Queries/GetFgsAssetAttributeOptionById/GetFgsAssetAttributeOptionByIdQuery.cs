using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Queries.GetFgsAssetAttributeOptionById;

public sealed record GetFgsAssetAttributeOptionByIdQuery(long Id) : IRequest<ApiResponse<FgsAssetAttributeOptionDetailDto>>;
