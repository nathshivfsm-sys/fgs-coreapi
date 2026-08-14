using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleDataAccesses.Commands.SyncFgsRoleDataAccesses;

public sealed record SyncFgsRoleDataAccessesCommand(FgsRoleDataAccessSyncDto Dto)
    : IRequest<ApiResponse<IReadOnlyList<FgsRoleDataAccessDetailDto>>>;
