using Fgs.Setup.Application.Features.GloLookups.Dtos;

namespace Fgs.Setup.Application.Abstractions.GloLookups;

public interface IGloCountryReadRepository
{
    Task<IReadOnlyList<GloCountryLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
}
