using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.Users.Commands.ResendFgsUserInvite;

public sealed class ResendFgsUserInviteCommandHandler(
    IFgsUserWriteService writeService,
    ILogger<ResendFgsUserInviteCommandHandler> logger)
    : IRequestHandler<ResendFgsUserInviteCommand, ApiResponse<FgsUserDetailDto>>
{
    public async Task<ApiResponse<FgsUserDetailDto>> Handle(
        ResendFgsUserInviteCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.ResendInviteAsync(request.Id, cancellationToken);
        logger.LogInformation("Resent invite for user {UserId}", result.Id);
        return ApiResponse<FgsUserDetailDto>.Ok(result);
    }
}
