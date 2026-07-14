using Fgs.Contracts.Api;
using Fgs.User.Application.Features.DataAccessScopes.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccessScopes.Queries.GetFgsDataAccessScopeById;

public sealed record GetFgsDataAccessScopeByIdQuery(long Id)
    : IRequest<ApiResponse<FgsDataAccessScopeDetailDto>>;
