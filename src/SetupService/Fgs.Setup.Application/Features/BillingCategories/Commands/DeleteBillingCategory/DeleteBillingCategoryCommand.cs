using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.BillingCategories.Commands.DeleteBillingCategory;

public sealed record DeleteBillingCategoryCommand(long Id)
    : IRequest<ApiResponse<BillingCategoryDetailDto>>;
