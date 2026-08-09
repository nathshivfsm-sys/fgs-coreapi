using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobCategories.Commands.DeleteJobCategory;

public sealed record DeleteJobCategoryCommand(long Id)
    : IRequest<ApiResponse<JobCategoryDetailDto>>;
