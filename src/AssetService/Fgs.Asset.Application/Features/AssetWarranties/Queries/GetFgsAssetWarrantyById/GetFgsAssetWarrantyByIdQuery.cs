using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetWarranties.Queries.GetFgsAssetWarrantyById;

public sealed record GetFgsAssetWarrantyByIdQuery(long Id) : IRequest<ApiResponse<FgsAssetWarrantyDetailDto>>;
