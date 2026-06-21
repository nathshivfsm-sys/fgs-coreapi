using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypes.Commands.DeleteJobType;

public sealed class DeleteJobTypeCommandHandler(
    IJobTypeWriteService writeService,
    ILogger<DeleteJobTypeCommandHandler> logger)
    : IRequestHandler<DeleteJobTypeCommand, ApiResponse<JobTypeDetailDto>>
{
    public async Task<ApiResponse<JobTypeDetailDto>> Handle(
        DeleteJobTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted job type {Id}", result.Id);
            return ApiResponse<JobTypeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete job type {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<JobTypeDetailDto>(ex);
        }
    }
}
