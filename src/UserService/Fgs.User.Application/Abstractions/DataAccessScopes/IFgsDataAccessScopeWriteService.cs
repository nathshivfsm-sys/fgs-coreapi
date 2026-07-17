using Fgs.User.Application.Features.DataAccessScopes.Dtos;

namespace Fgs.User.Application.Abstractions.DataAccessScopes;

public interface IFgsDataAccessScopeWriteService
{
    Task<FgsDataAccessScopeDetailDto> CreateAsync(
        FgsDataAccessScopeCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsDataAccessScopeDetailDto> UpdateAsync(
        long id,
        FgsDataAccessScopeUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsDataAccessScopeDetailDto> PatchAsync(
        long id,
        FgsDataAccessScopePatchDto dto,
        CancellationToken cancellationToken = default);
}
