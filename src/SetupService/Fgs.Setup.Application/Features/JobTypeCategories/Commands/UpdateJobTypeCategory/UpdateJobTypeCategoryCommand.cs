using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Commands.UpdateJobTypeCategory;

public sealed record UpdateJobTypeCategoryCommand(long Id, JobTypeCategoryUpdateDto Dto)
    : IRequest<ApiResponse<JobTypeCategoryDetailDto>>;
