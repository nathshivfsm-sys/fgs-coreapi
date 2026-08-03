using Fgs.User.Application.Features.ServiceSetups.Dtos;

namespace Fgs.User.Application.Abstractions.ServiceSetups;

public interface IFgsTenantServiceSetupWriteService
{
    Task<FgsTenantServiceSetupDetailDto> UpdateAsync(
        FgsTenantServiceSetupUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsTenantServiceSetupDetailDto> PatchAsync(
        FgsTenantServiceSetupPatchDto dto,
        CancellationToken cancellationToken = default);
}
