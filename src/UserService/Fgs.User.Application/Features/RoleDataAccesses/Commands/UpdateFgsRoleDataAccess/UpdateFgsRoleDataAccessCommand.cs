using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleDataAccesses.Commands.UpdateFgsRoleDataAccess;

public sealed record UpdateFgsRoleDataAccessCommand(long Id, FgsRoleDataAccessUpdateDto Dto)
    : IRequest<ApiResponse<FgsRoleDataAccessDetailDto>>;
