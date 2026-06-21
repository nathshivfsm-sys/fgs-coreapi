using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;

public interface IFgsSetupTaxAuthorityWriteService
{
    Task<FgsSetupTaxAuthorityDetailDto> CreateAsync(FgsSetupTaxAuthorityCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupTaxAuthorityDetailDto> UpdateAsync(long id, FgsSetupTaxAuthorityUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupTaxAuthorityDetailDto> PatchAsync(long id, FgsSetupTaxAuthorityPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupTaxAuthorityDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
