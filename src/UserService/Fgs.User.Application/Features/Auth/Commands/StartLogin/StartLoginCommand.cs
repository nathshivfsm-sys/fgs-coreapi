using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Commands.StartLogin;

public sealed record StartLoginCommand(string Email) : IRequest<ApiResponse<StartLoginResultDto>>;

public sealed record StartLoginResultDto(string RedirectUrl);
