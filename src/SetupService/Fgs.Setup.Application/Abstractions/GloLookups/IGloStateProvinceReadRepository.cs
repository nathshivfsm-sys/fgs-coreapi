using Fgs.Setup.Application.Features.GloLookups.Dtos;

namespace Fgs.Setup.Application.Abstractions.GloLookups;

public interface IGloStateProvinceReadRepository
{
    Task<IReadOnlyList<GloStateProvinceLookupDto>> LookupAsync(string countryCode, bool activeOnly = true, CancellationToken cancellationToken = default);
}
