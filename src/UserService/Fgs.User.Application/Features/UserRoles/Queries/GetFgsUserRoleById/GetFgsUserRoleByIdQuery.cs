using Fgs.Contracts.Api;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.UserRoles.Queries.GetFgsUserRoleById;

public sealed record GetFgsUserRoleByIdQuery(long Id) : IRequest<ApiResponse<FgsUserRoleDetailDto>>;
