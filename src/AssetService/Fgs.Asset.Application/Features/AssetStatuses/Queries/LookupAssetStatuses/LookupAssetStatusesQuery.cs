using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetStatuses.Queries.LookupAssetStatuses;

public sealed record LookupAssetStatusesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsAssetStatusLookupDto>>>;
