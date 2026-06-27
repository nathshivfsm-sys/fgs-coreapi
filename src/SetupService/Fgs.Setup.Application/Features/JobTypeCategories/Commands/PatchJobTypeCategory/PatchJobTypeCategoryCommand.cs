using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Commands.PatchJobTypeCategory;

public sealed record PatchJobTypeCategoryCommand(long Id, JobTypeCategoryPatchDto Dto)
    : IRequest<ApiResponse<JobTypeCategoryDetailDto>>;
