using Fgs.User.Application.Features.ServiceSetups.Dtos;

namespace Fgs.User.Application.Abstractions.ServiceSetups;

public interface IFgsTenantServiceSetupReadRepository
{
    Task<FgsTenantServiceSetupDetailDto?> GetCurrentAsync(CancellationToken cancellationToken = default);
}
