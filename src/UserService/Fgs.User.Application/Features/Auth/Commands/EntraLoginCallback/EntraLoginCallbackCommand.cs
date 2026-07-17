using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Commands.EntraLoginCallback;

public sealed record EntraLoginCallbackCommand(string Code, string State)
    : IRequest<ApiResponse<EntraLoginCallbackResultDto>>;

public sealed record EntraLoginCallbackResultDto(
    string AccessToken,
    string RedirectUrl);
