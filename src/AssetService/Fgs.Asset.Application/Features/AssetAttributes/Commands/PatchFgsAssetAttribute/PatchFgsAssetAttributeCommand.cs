using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributes.Commands.PatchFgsAssetAttribute;

public sealed record PatchFgsAssetAttributeCommand(long Id, FgsAssetAttributePatchDto Dto)
    : IRequest<ApiResponse<FgsAssetAttributeDetailDto>>;
