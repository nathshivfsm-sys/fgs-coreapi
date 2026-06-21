using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeSubCategories;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.UpdateJobTypeSubCategory;

public sealed class UpdateJobTypeSubCategoryCommandHandler(
    IJobTypeSubCategoryWriteService writeService,
    ILogger<UpdateJobTypeSubCategoryCommandHandler> logger)
    : IRequestHandler<UpdateJobTypeSubCategoryCommand, ApiResponse<JobTypeSubCategoryDetailDto>>
{
    public async Task<ApiResponse<JobTypeSubCategoryDetailDto>> Handle(
        UpdateJobTypeSubCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated job type subcategory {Id}", result.Id);
            return ApiResponse<JobTypeSubCategoryDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update job type subcategory {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<JobTypeSubCategoryDetailDto>(ex);
        }
    }
}
