using Fgs.User.Application.Features.PublicEndpoints.Dtos;

namespace Fgs.User.Application.Abstractions.PublicEndpoints;

public interface IFgsPublicEndpointWriteService
{
    Task<FgsPublicEndpointDetailDto> CreateAsync(
        FgsPublicEndpointCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsPublicEndpointDetailDto> UpdateAsync(
        long id,
        FgsPublicEndpointUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsPublicEndpointDetailDto> PatchAsync(
        long id,
        FgsPublicEndpointPatchDto dto,
        CancellationToken cancellationToken = default);
}
