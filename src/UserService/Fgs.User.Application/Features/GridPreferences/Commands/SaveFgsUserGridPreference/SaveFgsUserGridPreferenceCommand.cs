using System.Text.Json;
using Fgs.Contracts.Api;
using Fgs.User.Application.Features.GridPreferences.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.GridPreferences.Commands.SaveFgsUserGridPreference;

public sealed record SaveFgsUserGridPreferenceCommand(
    string GridCode,
    JsonElement GridConfiguration)
    : IRequest<ApiResponse<FgsUserGridPreferenceDto>>;
