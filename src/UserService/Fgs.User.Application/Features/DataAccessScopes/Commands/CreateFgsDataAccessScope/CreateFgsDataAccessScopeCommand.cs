using Fgs.Contracts.Api;
using Fgs.User.Application.Features.DataAccessScopes.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccessScopes.Commands.CreateFgsDataAccessScope;

public sealed record CreateFgsDataAccessScopeCommand(FgsDataAccessScopeCreateDto Dto)
    : IRequest<ApiResponse<FgsDataAccessScopeDetailDto>>;
