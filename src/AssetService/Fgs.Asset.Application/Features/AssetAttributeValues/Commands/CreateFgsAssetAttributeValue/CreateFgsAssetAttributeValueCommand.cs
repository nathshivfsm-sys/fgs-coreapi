using Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeValues.Commands.CreateFgsAssetAttributeValue;

public sealed record CreateFgsAssetAttributeValueCommand(FgsAssetAttributeValueCreateDto Dto)
    : IRequest<ApiResponse<FgsAssetAttributeValueDetailDto>>;
