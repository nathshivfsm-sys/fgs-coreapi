using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Users.Queries.GetFgsUserById;

public sealed record GetFgsUserByIdQuery(Guid Id) : IRequest<ApiResponse<FgsUserDetailDto>>;
