using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Queries.LookupJobTypeCategories;

public sealed record LookupJobTypeCategoriesQuery(bool ActiveOnly = true, long? JobTypeId = null)
    : IRequest<ApiResponse<IReadOnlyList<JobTypeCategoryLookupDto>>>;
