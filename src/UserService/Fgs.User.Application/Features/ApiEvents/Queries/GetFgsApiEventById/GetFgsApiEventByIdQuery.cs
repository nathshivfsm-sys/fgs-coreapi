using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiEvents.Queries.GetFgsApiEventById;

public sealed record GetFgsApiEventByIdQuery(long Id) : IRequest<ApiResponse<FgsApiEventDetailDto>>;
