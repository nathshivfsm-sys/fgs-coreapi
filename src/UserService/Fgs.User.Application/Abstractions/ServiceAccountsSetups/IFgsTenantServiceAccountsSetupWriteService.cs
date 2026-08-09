using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;

namespace Fgs.User.Application.Abstractions.ServiceAccountsSetups;

public interface IFgsTenantServiceAccountsSetupWriteService
{
    Task<FgsTenantServiceAccountsSetupDetailDto> UpdateAsync(
        FgsTenantServiceAccountsSetupUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsTenantServiceAccountsSetupDetailDto> PatchAsync(
        FgsTenantServiceAccountsSetupPatchDto dto,
        CancellationToken cancellationToken = default);
}
