using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Queries.LookupAssetAttributeOptions;

public sealed record LookupAssetAttributeOptionsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsAssetAttributeOptionLookupDto>>>;
