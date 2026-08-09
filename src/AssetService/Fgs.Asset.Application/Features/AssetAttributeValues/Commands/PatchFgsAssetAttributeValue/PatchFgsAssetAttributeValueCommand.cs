using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeValues.Commands.PatchFgsAssetAttributeValue;

public sealed record PatchFgsAssetAttributeValueCommand(long Id, FgsAssetAttributeValuePatchDto Dto)
    : IRequest<ApiResponse<FgsAssetAttributeValueDetailDto>>;
