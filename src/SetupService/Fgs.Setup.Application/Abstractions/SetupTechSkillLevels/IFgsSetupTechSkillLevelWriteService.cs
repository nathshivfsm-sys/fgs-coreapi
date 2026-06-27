using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;

public interface IFgsSetupTechSkillLevelWriteService
{
    Task<FgsSetupTechSkillLevelDetailDto> CreateAsync(FgsSetupTechSkillLevelCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupTechSkillLevelDetailDto> UpdateAsync(long id, FgsSetupTechSkillLevelUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupTechSkillLevelDetailDto> PatchAsync(long id, FgsSetupTechSkillLevelPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupTechSkillLevelDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
