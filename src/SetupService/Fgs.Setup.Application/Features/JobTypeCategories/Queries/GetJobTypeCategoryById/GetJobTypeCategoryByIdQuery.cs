using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Queries.GetJobTypeCategoryById;

public sealed record GetJobTypeCategoryByIdQuery(long Id)
    : IRequest<ApiResponse<JobTypeCategoryDetailDto>>;
