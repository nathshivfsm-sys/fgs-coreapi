using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Commands.DeleteJobTypeCategory;

public sealed record DeleteJobTypeCategoryCommand(long Id)
    : IRequest<ApiResponse<JobTypeCategoryDetailDto>>;
