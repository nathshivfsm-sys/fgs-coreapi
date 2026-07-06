using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetWarranties.Commands.UpdateFgsAssetWarranty;

public sealed record UpdateFgsAssetWarrantyCommand(long Id, FgsAssetWarrantyUpdateDto Dto)
    : IRequest<ApiResponse<FgsAssetWarrantyDetailDto>>;
