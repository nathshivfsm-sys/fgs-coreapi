using Fgs.Bff.Application.Features.Lookups.Dtos;

namespace Fgs.Bff.Application.Features.Lookups.Abstractions;

public interface ILookupGateway
{
    Task<LookupResultDto> GetLookupAsync(LookupRequestDto request, CancellationToken cancellationToken = default);
}
