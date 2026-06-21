using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Queries.GetJobTypeSubCategoryById;

public sealed record GetJobTypeSubCategoryByIdQuery(long Id)
    : IRequest<ApiResponse<JobTypeSubCategoryDetailDto>>;
