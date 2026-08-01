using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.Users.Commands.InviteFgsUser;

public sealed class InviteFgsUserCommandHandler(
    IFgsUserWriteService writeService,
    ILogger<InviteFgsUserCommandHandler> logger)
    : IRequestHandler<InviteFgsUserCommand, ApiResponse<FgsUserDetailDto>>
{
    public async Task<ApiResponse<FgsUserDetailDto>> Handle(
        InviteFgsUserCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.InviteAsync(request.Dto, cancellationToken);
        logger.LogInformation("Invited user {UserId} with email {Email}", result.Id, result.Email);
        return ApiResponse<FgsUserDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
