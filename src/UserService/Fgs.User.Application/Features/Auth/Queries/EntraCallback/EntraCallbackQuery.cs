using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Queries.EntraCallback;

public sealed record EntraCallbackQuery(string Code, string State) : IRequest<ApiResponse<EntraCallbackResultDto>>;

public sealed record EntraCallbackResultDto(
    string AccessToken,
    string RedirectUrl);

