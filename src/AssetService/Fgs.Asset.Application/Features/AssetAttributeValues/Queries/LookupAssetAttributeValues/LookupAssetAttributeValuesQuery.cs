using Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeValues.Queries.LookupAssetAttributeValues;

public sealed record LookupAssetAttributeValuesQuery()
    : IRequest<ApiResponse<IReadOnlyList<FgsAssetAttributeValueLookupDto>>>;
