using Fgs.Setup.Application.Features.JobCategories.Dtos;

namespace Fgs.Setup.Application.Abstractions.JobCategories;

public interface IJobCategoryWriteService
{
    Task<JobCategoryDetailDto> CreateAsync(JobCategoryCreateDto dto, CancellationToken cancellationToken = default);

    Task<JobCategoryDetailDto> UpdateAsync(long id, JobCategoryUpdateDto dto, CancellationToken cancellationToken = default);

    Task<JobCategoryDetailDto> PatchAsync(long id, JobCategoryPatchDto dto, CancellationToken cancellationToken = default);

    Task<JobCategoryDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
