using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Users.Commands.InviteFgsUser;

public sealed record InviteFgsUserCommand(IReadOnlyList<FgsUserInviteDto> Invites)
    : IRequest<ApiResponse<IReadOnlyList<FgsUserDetailDto>>>;
