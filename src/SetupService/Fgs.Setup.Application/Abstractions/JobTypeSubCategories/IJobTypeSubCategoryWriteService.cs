using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;

namespace Fgs.Setup.Application.Abstractions.JobTypeSubCategories;

public interface IJobTypeSubCategoryWriteService
{
    Task<JobTypeSubCategoryDetailDto> CreateAsync(JobTypeSubCategoryCreateDto dto, CancellationToken cancellationToken = default);

    Task<JobTypeSubCategoryDetailDto> UpdateAsync(long id, JobTypeSubCategoryUpdateDto dto, CancellationToken cancellationToken = default);

    Task<JobTypeSubCategoryDetailDto> PatchAsync(long id, JobTypeSubCategoryPatchDto dto, CancellationToken cancellationToken = default);

    Task<JobTypeSubCategoryDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
