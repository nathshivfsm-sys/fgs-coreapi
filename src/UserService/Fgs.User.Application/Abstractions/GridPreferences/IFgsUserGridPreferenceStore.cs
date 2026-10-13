using Fgs.User.Application.Features.GridPreferences.Dtos;

namespace Fgs.User.Application.Abstractions.GridPreferences;

public interface IFgsUserGridPreferenceStore
{
    Task<FgsUserGridPreferenceDto> GetAsync(string gridCode, CancellationToken cancellationToken = default);

    Task<FgsUserGridPreferenceDto> UpsertAsync(
        string gridCode,
        IReadOnlyList<string> columns,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string gridCode, CancellationToken cancellationToken = default);
}
