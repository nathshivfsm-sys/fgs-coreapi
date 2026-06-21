using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeSubCategories;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.DeleteJobTypeSubCategory;

public sealed class DeleteJobTypeSubCategoryCommandHandler(
    IJobTypeSubCategoryWriteService writeService,
    ILogger<DeleteJobTypeSubCategoryCommandHandler> logger)
    : IRequestHandler<DeleteJobTypeSubCategoryCommand, ApiResponse<JobTypeSubCategoryDetailDto>>
{
    public async Task<ApiResponse<JobTypeSubCategoryDetailDto>> Handle(
        DeleteJobTypeSubCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted job type subcategory {Id}", result.Id);
            return ApiResponse<JobTypeSubCategoryDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete job type subcategory {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<JobTypeSubCategoryDetailDto>(ex);
        }
    }
}
