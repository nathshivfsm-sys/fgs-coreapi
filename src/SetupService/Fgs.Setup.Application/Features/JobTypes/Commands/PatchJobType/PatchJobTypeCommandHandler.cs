using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypes.Commands.PatchJobType;

public sealed class PatchJobTypeCommandHandler(
    IJobTypeWriteService writeService,
    ILogger<PatchJobTypeCommandHandler> logger)
    : IRequestHandler<PatchJobTypeCommand, ApiResponse<JobTypeDetailDto>>
{
    public async Task<ApiResponse<JobTypeDetailDto>> Handle(
        PatchJobTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd job type {Id}", result.Id);
            return ApiResponse<JobTypeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch job type {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<JobTypeDetailDto>(ex);
        }
    }
}
