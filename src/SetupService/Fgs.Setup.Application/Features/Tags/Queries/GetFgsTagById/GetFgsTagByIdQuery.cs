using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Tags.Queries.GetFgsTagById;

public sealed record GetFgsTagByIdQuery(long Id)
    : IRequest<ApiResponse<FgsTagDetailDto>>;
