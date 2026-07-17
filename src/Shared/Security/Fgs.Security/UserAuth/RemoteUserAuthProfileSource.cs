using Fgs.Contracts.Api;
using Fgs.Contracts.Auth;
using Fgs.Contracts.Clients;

namespace Fgs.Security.UserAuth;

public sealed class RemoteUserAuthProfileSource(IUserAuthProfileClient client) : IUserAuthProfileSource
{
    public async Task<UserAuthProfileDto?> LoadByEntraObjectIdAsync(
        string entraObjectId,
        CancellationToken cancellationToken = default)
    {
        var response = await client.GetByEntraObjectIdAsync(entraObjectId, cancellationToken);
        return response.Success ? response.Data : null;
    }
}
