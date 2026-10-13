using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.User.Application.Features.GridPreferences.Commands.DeleteFgsUserGridPreference;

public sealed record DeleteFgsUserGridPreferenceCommand(string GridCode)
    : IRequest<ApiResponse<object>>;
