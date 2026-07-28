using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;

namespace Fgs.Setup.Application.Abstractions.JobTypeTasks;

public interface IJobTypeTaskWriteService
{
    Task<JobTypeTaskDetailDto> CreateAsync(JobTypeTaskCreateDto dto, CancellationToken cancellationToken = default);

    Task<JobTypeTaskDetailDto> UpdateAsync(long id, JobTypeTaskUpdateDto dto, CancellationToken cancellationToken = default);

    Task<JobTypeTaskDetailDto> PatchAsync(long id, JobTypeTaskPatchDto dto, CancellationToken cancellationToken = default);

    Task<JobTypeTaskDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
