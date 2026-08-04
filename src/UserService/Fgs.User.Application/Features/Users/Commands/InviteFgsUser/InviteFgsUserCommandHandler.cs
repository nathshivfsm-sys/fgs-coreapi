using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.Users.Commands.InviteFgsUser;

public sealed class InviteFgsUserCommandHandler(
    IFgsUserWriteService writeService,
    ILogger<InviteFgsUserCommandHandler> logger)
    : IRequestHandler<InviteFgsUserCommand, ApiResponse<IReadOnlyList<FgsUserDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsUserDetailDto>>> Handle(
        InviteFgsUserCommand request,
        CancellationToken cancellationToken)
    {
        var results = await writeService.InviteAsync(request.Invites, cancellationToken);
        logger.LogInformation("Invited {Count} user(s)", results.Count);
        return ApiResponse<IReadOnlyList<FgsUserDetailDto>>.Ok(results, ApiStatusCodes.Created);
    }
}
