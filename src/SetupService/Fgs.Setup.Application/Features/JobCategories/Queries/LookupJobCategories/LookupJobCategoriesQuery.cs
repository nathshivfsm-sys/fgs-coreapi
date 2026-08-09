using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobCategories.Queries.LookupJobCategories;

public sealed record LookupJobCategoriesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<JobCategoryLookupDto>>>;
