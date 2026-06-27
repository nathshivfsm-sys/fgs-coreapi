using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.CreateJobTypeSubCategory;

public sealed record CreateJobTypeSubCategoryCommand(JobTypeSubCategoryCreateDto Dto)
    : IRequest<ApiResponse<JobTypeSubCategoryDetailDto>>;
