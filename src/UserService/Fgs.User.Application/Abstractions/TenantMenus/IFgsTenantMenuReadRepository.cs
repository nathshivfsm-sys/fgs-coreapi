using Fgs.User.Application.Features.TenantMenus.Dtos;

namespace Fgs.User.Application.Abstractions.TenantMenus;

public interface IFgsTenantMenuReadRepository
{
    Task<FgsTenantMenuDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsTenantMenuDetailDto>> ListAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsTenantMenuLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByMenuIdAsync(
        int menuId,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByMenuCodeAsync(
        string menuCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
