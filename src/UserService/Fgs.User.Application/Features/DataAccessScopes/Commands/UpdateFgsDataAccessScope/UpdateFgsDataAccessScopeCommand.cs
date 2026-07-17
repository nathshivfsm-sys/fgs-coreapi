using Fgs.Contracts.Api;
using Fgs.User.Application.Features.DataAccessScopes.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccessScopes.Commands.UpdateFgsDataAccessScope;

public sealed record UpdateFgsDataAccessScopeCommand(long Id, FgsDataAccessScopeUpdateDto Dto)
    : IRequest<ApiResponse<FgsDataAccessScopeDetailDto>>;
