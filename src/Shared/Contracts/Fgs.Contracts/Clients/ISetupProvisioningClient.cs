using Fgs.Contracts.Requests;
using Refit;

namespace Fgs.Contracts.Clients;

public interface ISetupProvisioningClient
{
    [Post("/api/v1/tenant-provisioning")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> ProvisionTenantAsync(
        [Body] ProvisionTenantRequest request,
        CancellationToken cancellationToken = default);
}
