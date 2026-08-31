using Fgs.User.Application.Features.TenantMenus.Dtos;

namespace Fgs.User.Application.Abstractions.TenantMenus;

public interface IFgsTenantMenuReadRepository
{
    Task<IReadOnlyList<FgsTenantMenuDetailDto>> ListAsync(CancellationToken cancellationToken = default);
}
