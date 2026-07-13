using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Roles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Roles.Queries.GetFgsRoleById;

public sealed record GetFgsRoleByIdQuery(long Id) : IRequest<ApiResponse<FgsRoleDetailDto>>;
