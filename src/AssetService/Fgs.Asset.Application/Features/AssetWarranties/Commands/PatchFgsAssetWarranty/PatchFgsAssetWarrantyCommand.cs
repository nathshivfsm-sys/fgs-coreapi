using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetWarranties.Commands.PatchFgsAssetWarranty;

public sealed record PatchFgsAssetWarrantyCommand(long Id, FgsAssetWarrantyPatchDto Dto)
    : IRequest<ApiResponse<FgsAssetWarrantyDetailDto>>;
