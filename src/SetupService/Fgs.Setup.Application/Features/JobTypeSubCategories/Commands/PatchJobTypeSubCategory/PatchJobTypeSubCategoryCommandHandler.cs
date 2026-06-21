using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeSubCategories;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.PatchJobTypeSubCategory;

public sealed class PatchJobTypeSubCategoryCommandHandler(
    IJobTypeSubCategoryWriteService writeService,
    ILogger<PatchJobTypeSubCategoryCommandHandler> logger)
    : IRequestHandler<PatchJobTypeSubCategoryCommand, ApiResponse<JobTypeSubCategoryDetailDto>>
{
    public async Task<ApiResponse<JobTypeSubCategoryDetailDto>> Handle(
        PatchJobTypeSubCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd job type subcategory {Id}", result.Id);
            return ApiResponse<JobTypeSubCategoryDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch job type subcategory {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<JobTypeSubCategoryDetailDto>(ex);
        }
    }
}
