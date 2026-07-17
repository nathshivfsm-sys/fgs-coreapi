using Fgs.User.Application.Features.ApiEvents.Dtos;

namespace Fgs.User.Application.Abstractions.ApiEvents;

public interface IFgsApiEventWriteService
{
    Task<FgsApiEventDetailDto> CreateAsync(FgsApiEventCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsApiEventDetailDto> UpdateAsync(long id, FgsApiEventUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsApiEventDetailDto> PatchAsync(long id, FgsApiEventPatchDto dto, CancellationToken cancellationToken = default);
}
