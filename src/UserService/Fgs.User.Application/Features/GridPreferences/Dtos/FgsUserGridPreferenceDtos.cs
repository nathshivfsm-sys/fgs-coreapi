namespace Fgs.User.Application.Features.GridPreferences.Dtos;

public sealed record FgsUserGridPreferenceDto(
    string GridCode,
    IReadOnlyList<string> Columns);

public sealed record FgsUserGridPreferenceWriteDto(IReadOnlyList<string>? Columns);
