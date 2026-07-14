using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.DataAccessScopes;
using Fgs.User.Application.Features.DataAccessScopes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.DataAccessScopes.Commands.PatchFgsDataAccessScope;

public sealed class PatchFgsDataAccessScopeCommandHandler(
    IFgsDataAccessScopeWriteService writeService,
    ILogger<PatchFgsDataAccessScopeCommandHandler> logger)
    : IRequestHandler<PatchFgsDataAccessScopeCommand, ApiResponse<FgsDataAccessScopeDetailDto>>
{
    public async Task<ApiResponse<FgsDataAccessScopeDetailDto>> Handle(
        PatchFgsDataAccessScopeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched data access scope {ScopeId}", result.Id);
        return ApiResponse<FgsDataAccessScopeDetailDto>.Ok(result);
    }
}
