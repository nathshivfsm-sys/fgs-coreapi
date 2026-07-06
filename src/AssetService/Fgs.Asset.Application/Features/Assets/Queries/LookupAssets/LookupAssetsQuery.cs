using Fgs.Asset.Application.Features.Assets.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.Assets.Queries.LookupAssets;

public sealed record LookupAssetsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsAssetLookupDto>>>;
