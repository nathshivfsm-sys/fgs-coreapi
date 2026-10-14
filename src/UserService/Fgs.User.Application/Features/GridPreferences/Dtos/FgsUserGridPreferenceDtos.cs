using System.Text.Json;

namespace Fgs.User.Application.Features.GridPreferences.Dtos;

public sealed record FgsUserGridPreferenceDto(
    string GridCode,
    JsonElement GridConfiguration);

public sealed record FgsUserGridPreferenceWriteDto(JsonElement GridConfiguration);
