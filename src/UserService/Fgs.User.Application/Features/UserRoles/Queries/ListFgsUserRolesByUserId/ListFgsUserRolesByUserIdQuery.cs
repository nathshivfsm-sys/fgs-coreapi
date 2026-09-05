using Fgs.Contracts.Api;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.UserRoles.Queries.ListFgsUserRolesByUserId;

public sealed record ListFgsUserRolesByUserIdQuery(Guid UserId)
    : IRequest<ApiResponse<IReadOnlyList<FgsUserRoleDetailDto>>>;
