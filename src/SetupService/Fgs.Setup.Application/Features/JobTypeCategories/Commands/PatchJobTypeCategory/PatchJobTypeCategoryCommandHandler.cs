using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Commands.PatchJobTypeCategory;

public sealed class PatchJobTypeCategoryCommandHandler(
    IJobTypeCategoryWriteService writeService,
    ILogger<PatchJobTypeCategoryCommandHandler> logger)
    : IRequestHandler<PatchJobTypeCategoryCommand, ApiResponse<JobTypeCategoryDetailDto>>
{
    public async Task<ApiResponse<JobTypeCategoryDetailDto>> Handle(
        PatchJobTypeCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd job type category {Id}", result.Id);
            return ApiResponse<JobTypeCategoryDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch job type category {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<JobTypeCategoryDetailDto>(ex);
        }
    }
}
