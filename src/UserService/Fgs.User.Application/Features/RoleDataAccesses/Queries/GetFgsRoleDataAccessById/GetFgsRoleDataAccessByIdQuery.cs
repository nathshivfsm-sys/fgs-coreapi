using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleDataAccesses.Queries.GetFgsRoleDataAccessById;

public sealed record GetFgsRoleDataAccessByIdQuery(long Id) : IRequest<ApiResponse<FgsRoleDataAccessDetailDto>>;
