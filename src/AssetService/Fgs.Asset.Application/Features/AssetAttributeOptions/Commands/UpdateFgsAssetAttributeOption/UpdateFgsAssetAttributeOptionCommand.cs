using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Commands.UpdateFgsAssetAttributeOption;

public sealed record UpdateFgsAssetAttributeOptionCommand(long Id, FgsAssetAttributeOptionUpdateDto Dto)
    : IRequest<ApiResponse<FgsAssetAttributeOptionDetailDto>>;
