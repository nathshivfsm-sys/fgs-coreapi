using Fgs.Setup.Application.Features.GloLookups.Dtos;

namespace Fgs.Setup.Application.Abstractions.GloLookups;

public interface IGloLanguageReadRepository
{
    Task<IReadOnlyList<GloLanguageLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
}
