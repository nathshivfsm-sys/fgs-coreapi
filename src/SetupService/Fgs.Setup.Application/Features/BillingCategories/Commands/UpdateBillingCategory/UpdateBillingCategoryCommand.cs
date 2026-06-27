using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.BillingCategories.Commands.UpdateBillingCategory;

public sealed record UpdateBillingCategoryCommand(long Id, BillingCategoryUpdateDto Dto)
    : IRequest<ApiResponse<BillingCategoryDetailDto>>;
