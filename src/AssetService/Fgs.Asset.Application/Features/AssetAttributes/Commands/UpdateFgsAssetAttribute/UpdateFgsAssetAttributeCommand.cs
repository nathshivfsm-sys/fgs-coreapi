using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributes.Commands.UpdateFgsAssetAttribute;

public sealed record UpdateFgsAssetAttributeCommand(long Id, FgsAssetAttributeUpdateDto Dto)
    : IRequest<ApiResponse<FgsAssetAttributeDetailDto>>;
