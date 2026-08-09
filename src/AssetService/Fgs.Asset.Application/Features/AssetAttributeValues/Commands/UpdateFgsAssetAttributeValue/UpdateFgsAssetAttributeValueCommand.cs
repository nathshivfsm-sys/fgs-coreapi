using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeValues.Commands.UpdateFgsAssetAttributeValue;

public sealed record UpdateFgsAssetAttributeValueCommand(long Id, FgsAssetAttributeValueUpdateDto Dto)
    : IRequest<ApiResponse<FgsAssetAttributeValueDetailDto>>;
