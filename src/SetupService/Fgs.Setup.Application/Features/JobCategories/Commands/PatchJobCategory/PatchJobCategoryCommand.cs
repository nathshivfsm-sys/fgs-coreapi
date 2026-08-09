using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobCategories.Commands.PatchJobCategory;

public sealed record PatchJobCategoryCommand(long Id, JobCategoryPatchDto Dto)
    : IRequest<ApiResponse<JobCategoryDetailDto>>;
