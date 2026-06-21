using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.PatchJobTypeSubCategory;

public sealed record PatchJobTypeSubCategoryCommand(long Id, JobTypeSubCategoryPatchDto Dto)
    : IRequest<ApiResponse<JobTypeSubCategoryDetailDto>>;
