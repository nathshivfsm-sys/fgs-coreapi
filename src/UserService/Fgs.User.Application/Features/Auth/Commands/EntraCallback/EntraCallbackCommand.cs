using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Commands.EntraCallback;

public sealed record EntraCallbackCommand(string Code, string State) : IRequest<ApiResponse<EntraCallbackResultDto>>;

public sealed record EntraCallbackResultDto(
    string AccessToken,
    string RedirectUrl);
