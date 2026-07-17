using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleDataAccesses.Commands.CreateFgsRoleDataAccess;

public sealed record CreateFgsRoleDataAccessCommand(FgsRoleDataAccessCreateDto Dto)
    : IRequest<ApiResponse<FgsRoleDataAccessDetailDto>>;
