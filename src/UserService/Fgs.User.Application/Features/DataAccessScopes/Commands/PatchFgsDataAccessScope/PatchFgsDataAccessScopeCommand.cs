using Fgs.Contracts.Api;
using Fgs.User.Application.Features.DataAccessScopes.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccessScopes.Commands.PatchFgsDataAccessScope;

public sealed record PatchFgsDataAccessScopeCommand(long Id, FgsDataAccessScopePatchDto Dto)
    : IRequest<ApiResponse<FgsDataAccessScopeDetailDto>>;
