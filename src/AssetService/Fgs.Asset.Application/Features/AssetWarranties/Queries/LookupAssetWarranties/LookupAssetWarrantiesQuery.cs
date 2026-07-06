using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetWarranties.Queries.LookupAssetWarranties;

public sealed record LookupAssetWarrantiesQuery()
    : IRequest<ApiResponse<IReadOnlyList<FgsAssetWarrantyLookupDto>>>;
