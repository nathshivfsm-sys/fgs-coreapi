using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.BillingCategories.Queries.GetBillingCategoryById;

public sealed record GetBillingCategoryByIdQuery(long Id)
    : IRequest<ApiResponse<BillingCategoryDetailDto>>;
