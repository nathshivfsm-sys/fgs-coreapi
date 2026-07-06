using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Commands.CreateFgsAssetAttributeOption;

public sealed record CreateFgsAssetAttributeOptionCommand(FgsAssetAttributeOptionCreateDto Dto)
    : IRequest<ApiResponse<FgsAssetAttributeOptionDetailDto>>;
