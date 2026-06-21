using Fgs.Setup.Application.Features.JobTypes.Dtos;

namespace Fgs.Setup.Application.Abstractions.JobTypes;

public interface IJobTypeWriteService
{
    Task<JobTypeDetailDto> CreateAsync(JobTypeCreateDto dto, CancellationToken cancellationToken = default);

    Task<JobTypeDetailDto> UpdateAsync(long id, JobTypeUpdateDto dto, CancellationToken cancellationToken = default);

    Task<JobTypeDetailDto> PatchAsync(long id, JobTypePatchDto dto, CancellationToken cancellationToken = default);

    Task<JobTypeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
