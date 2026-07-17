using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.DataAccessScopes;
using Fgs.User.Application.Features.DataAccessScopes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.DataAccessScopes.Commands.CreateFgsDataAccessScope;

public sealed class CreateFgsDataAccessScopeCommandHandler(
    IFgsDataAccessScopeWriteService writeService,
    ILogger<CreateFgsDataAccessScopeCommandHandler> logger)
    : IRequestHandler<CreateFgsDataAccessScopeCommand, ApiResponse<FgsDataAccessScopeDetailDto>>
{
    public async Task<ApiResponse<FgsDataAccessScopeDetailDto>> Handle(
        CreateFgsDataAccessScopeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created data access scope {ScopeId} for data access {DataAccessId}",
            result.Id,
            result.FgsDataAccessId);
        return ApiResponse<FgsDataAccessScopeDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
