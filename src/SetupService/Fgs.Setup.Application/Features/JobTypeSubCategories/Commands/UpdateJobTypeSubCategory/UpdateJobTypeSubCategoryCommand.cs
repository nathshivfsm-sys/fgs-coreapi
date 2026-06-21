using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.UpdateJobTypeSubCategory;

public sealed record UpdateJobTypeSubCategoryCommand(long Id, JobTypeSubCategoryUpdateDto Dto)
    : IRequest<ApiResponse<JobTypeSubCategoryDetailDto>>;
