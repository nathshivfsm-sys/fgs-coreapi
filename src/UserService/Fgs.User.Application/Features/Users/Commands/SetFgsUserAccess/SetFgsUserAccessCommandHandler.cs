using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Users;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.Users.Commands.SetFgsUserAccess;

public sealed class SetFgsUserAccessCommandHandler(
    IFgsUserWriteService writeService,
    ILogger<SetFgsUserAccessCommandHandler> logger)
    : IRequestHandler<SetFgsUserAccessCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        SetFgsUserAccessCommand request,
        CancellationToken cancellationToken)
    {
        await writeService.SetAccessAsync(request.UserId, request.IsActive, cancellationToken);
        logger.LogInformation(
            "Set user {UserId} access IsActive={IsActive}",
            request.UserId,
            request.IsActive);
        return ApiResponse<object>.Ok(new object());
    }
}
