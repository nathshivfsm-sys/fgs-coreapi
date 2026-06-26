using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;

public interface IFgsSetupTechSkillLevelReadRepository
{
    Task<FgsSetupTechSkillLevelDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSetupTechSkillLevelSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupTechSkillLevelListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSetupTechSkillLevelLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(
        string code,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
