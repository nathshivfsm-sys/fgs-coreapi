using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;

namespace Fgs.Setup.Application.Abstractions.JobTypeCategories;

public interface IJobTypeCategoryWriteService
{
    Task<JobTypeCategoryDetailDto> CreateAsync(JobTypeCategoryCreateDto dto, CancellationToken cancellationToken = default);

    Task<JobTypeCategoryDetailDto> UpdateAsync(long id, JobTypeCategoryUpdateDto dto, CancellationToken cancellationToken = default);

    Task<JobTypeCategoryDetailDto> PatchAsync(long id, JobTypeCategoryPatchDto dto, CancellationToken cancellationToken = default);

    Task<JobTypeCategoryDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
