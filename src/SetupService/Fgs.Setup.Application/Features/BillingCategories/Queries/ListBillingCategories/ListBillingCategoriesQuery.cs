using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.BillingCategories.Queries.ListBillingCategories;

public sealed record ListBillingCategoriesQuery(
    SetupListQuery Query, BillingCategoryListFilters Filters)
    : IRequest<ApiResponse<PagedResult<BillingCategorySummaryDto>>>;
