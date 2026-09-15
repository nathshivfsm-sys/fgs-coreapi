using Fgs.Setup.Application.Features.GloLookups.Dtos;

namespace Fgs.Setup.Application.Abstractions.GloLookups;

public interface IGloLocationTypeReadRepository
{
    Task<IReadOnlyList<GloLocationTypeLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
}
