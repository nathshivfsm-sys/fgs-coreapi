using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.Assets.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.Assets.Queries.GetFgsAssetById;

public sealed record GetFgsAssetByIdQuery(long Id) : IRequest<ApiResponse<FgsAssetDetailDto>>;
