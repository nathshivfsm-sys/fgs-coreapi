using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobCategories.Commands.CreateJobCategory;

public sealed record CreateJobCategoryCommand(JobCategoryCreateDto Dto)
    : IRequest<ApiResponse<JobCategoryDetailDto>>;
