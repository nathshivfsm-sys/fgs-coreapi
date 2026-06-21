using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypes.Commands.CreateJobType;

public sealed class CreateJobTypeCommandHandler(
    IJobTypeWriteService writeService,
    ILogger<CreateJobTypeCommandHandler> logger)
    : IRequestHandler<CreateJobTypeCommand, ApiResponse<JobTypeDetailDto>>
{
    public async Task<ApiResponse<JobTypeDetailDto>> Handle(
        CreateJobTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created job type {Id} with code {JobTypeCode}", result.Id, result.JobTypeCode);
            return ApiResponse<JobTypeDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create job type");
            return CatalogCrudExceptionMapper.MapException<JobTypeDetailDto>(ex);
        }
    }
}
