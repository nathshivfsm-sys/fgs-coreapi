using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.DeleteJobTypeSubCategory;

public sealed record DeleteJobTypeSubCategoryCommand(long Id)
    : IRequest<ApiResponse<JobTypeSubCategoryDetailDto>>;
