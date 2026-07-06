using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetTypes.Queries.LookupAssetTypes;

public sealed record LookupAssetTypesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsAssetTypeLookupDto>>>;
