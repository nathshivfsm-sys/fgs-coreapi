using Fgs.Setup.Application.Features.GloLookups.Dtos;

namespace Fgs.Setup.Application.Abstractions.GloLookups;

public interface IGloInventoryTransactionTypeReadRepository
{
    Task<IReadOnlyList<GloInventoryTransactionTypeLookupDto>> LookupAsync(int? sourceTypeId = null, bool activeOnly = true, CancellationToken cancellationToken = default);
}
