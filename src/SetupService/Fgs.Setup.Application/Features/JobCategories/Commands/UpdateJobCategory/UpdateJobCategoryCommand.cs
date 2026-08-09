using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobCategories.Commands.UpdateJobCategory;

public sealed record UpdateJobCategoryCommand(long Id, JobCategoryUpdateDto Dto)
    : IRequest<ApiResponse<JobCategoryDetailDto>>;
