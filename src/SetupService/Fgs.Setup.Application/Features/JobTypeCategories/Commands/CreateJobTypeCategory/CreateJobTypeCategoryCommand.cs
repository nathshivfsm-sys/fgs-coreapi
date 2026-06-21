using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Commands.CreateJobTypeCategory;

public sealed record CreateJobTypeCategoryCommand(JobTypeCategoryCreateDto Dto)
    : IRequest<ApiResponse<JobTypeCategoryDetailDto>>;
