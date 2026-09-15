using Fgs.Setup.Application.Features.GloLookups.Dtos;

namespace Fgs.Setup.Application.Abstractions.GloLookups;

public interface IGloAccountingIntegrationTypeReadRepository
{
    Task<IReadOnlyList<GloAccountingIntegrationTypeLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
}
