using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.BillingCategories.Commands.CreateBillingCategory;

public sealed record CreateBillingCategoryCommand(BillingCategoryCreateDto Dto)
    : IRequest<ApiResponse<BillingCategoryDetailDto>>;
