using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.BillingCategories.Queries.LookupBillingCategories;

public sealed record LookupBillingCategoriesQuery(bool ActiveOnly = true, bool? ShowToFieldTech = null)
    : IRequest<ApiResponse<IReadOnlyList<BillingCategoryLookupDto>>>;
