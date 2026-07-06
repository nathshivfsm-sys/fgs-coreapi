using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetWarranties.Commands.CreateFgsAssetWarranty;

public sealed record CreateFgsAssetWarrantyCommand(FgsAssetWarrantyCreateDto Dto)
    : IRequest<ApiResponse<FgsAssetWarrantyDetailDto>>;
