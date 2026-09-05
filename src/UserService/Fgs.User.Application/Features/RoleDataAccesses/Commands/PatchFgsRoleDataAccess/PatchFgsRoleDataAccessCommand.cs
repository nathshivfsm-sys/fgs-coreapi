using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleDataAccesses.Commands.PatchFgsRoleDataAccess;

public sealed record PatchFgsRoleDataAccessCommand(long Id, FgsRoleDataAccessPatchDto Dto)
    : IRequest<ApiResponse<FgsRoleDataAccessDetailDto>>;
