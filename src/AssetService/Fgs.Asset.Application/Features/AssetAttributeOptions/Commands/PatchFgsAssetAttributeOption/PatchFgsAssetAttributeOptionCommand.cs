using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Commands.PatchFgsAssetAttributeOption;

public sealed record PatchFgsAssetAttributeOptionCommand(long Id, FgsAssetAttributeOptionPatchDto Dto)
    : IRequest<ApiResponse<FgsAssetAttributeOptionDetailDto>>;
