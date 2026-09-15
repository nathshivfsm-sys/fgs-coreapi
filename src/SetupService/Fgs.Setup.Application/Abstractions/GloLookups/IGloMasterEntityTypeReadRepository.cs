using Fgs.Setup.Application.Features.GloLookups.Dtos;

namespace Fgs.Setup.Application.Abstractions.GloLookups;

public interface IGloMasterEntityTypeReadRepository
{
    Task<IReadOnlyList<GloMasterEntityTypeLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
}
