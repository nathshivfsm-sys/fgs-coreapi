using Fgs.Setup.Application.Features.GloLookups.Dtos;

namespace Fgs.Setup.Application.Abstractions.GloLookups;

public interface IGloUnitOfMeasureReadRepository
{
    Task<IReadOnlyList<GloUnitOfMeasureLookupDto>> LookupAsync(string? unitType = null, bool activeOnly = true, CancellationToken cancellationToken = default);
}
