using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.BillingCategories.Queries.LookupBillingCategories;

public sealed record LookupBillingCategoriesQuery(
    bool ActiveOnly = true,
    bool? ShowToFieldTech = null,
    bool? AllowToPick = null)
    : IRequest<ApiResponse<IReadOnlyList<BillingCategoryLookupDto>>>;
