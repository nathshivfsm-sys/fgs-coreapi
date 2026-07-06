using Fgs.Asset.Application.Features.AssetModels.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetModels.Queries.LookupAssetModels;

public sealed record LookupAssetModelsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsAssetModelLookupDto>>>;
