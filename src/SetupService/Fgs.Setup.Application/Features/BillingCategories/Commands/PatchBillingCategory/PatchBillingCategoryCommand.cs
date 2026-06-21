using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.BillingCategories.Commands.PatchBillingCategory;

public sealed record PatchBillingCategoryCommand(long Id, BillingCategoryPatchDto Dto)
    : IRequest<ApiResponse<BillingCategoryDetailDto>>;
