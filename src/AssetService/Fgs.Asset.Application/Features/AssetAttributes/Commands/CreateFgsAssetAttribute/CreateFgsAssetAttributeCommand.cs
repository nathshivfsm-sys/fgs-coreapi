using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributes.Commands.CreateFgsAssetAttribute;

public sealed record CreateFgsAssetAttributeCommand(FgsAssetAttributeCreateDto Dto)
    : IRequest<ApiResponse<FgsAssetAttributeDetailDto>>;
