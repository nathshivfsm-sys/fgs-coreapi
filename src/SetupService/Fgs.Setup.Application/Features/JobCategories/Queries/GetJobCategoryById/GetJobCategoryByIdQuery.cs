using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobCategories.Queries.GetJobCategoryById;

public sealed record GetJobCategoryByIdQuery(long Id)
    : IRequest<ApiResponse<JobCategoryDetailDto>>;
