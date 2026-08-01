using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Users.Commands.InviteFgsUser;

public sealed record InviteFgsUserCommand(FgsUserInviteDto Dto) : IRequest<ApiResponse<FgsUserDetailDto>>;
