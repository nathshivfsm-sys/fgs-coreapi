using Fgs.Setup.Application.Features.GloLookups.Dtos;

namespace Fgs.Setup.Application.Abstractions.GloLookups;

public interface IGloInventoryTransactionSourceTypeReadRepository
{
    Task<IReadOnlyList<GloInventoryTransactionSourceTypeLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
}
