using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Queries.LookupJobTypeSubCategories;

public sealed record LookupJobTypeSubCategoriesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<JobTypeSubCategoryLookupDto>>>;
