using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributes.Queries.LookupAssetAttributes;

public sealed record LookupAssetAttributesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsAssetAttributeLookupDto>>>;
