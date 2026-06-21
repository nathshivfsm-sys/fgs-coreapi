using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Commands.DeleteJobTypeCategory;

public sealed class DeleteJobTypeCategoryCommandHandler(
    IJobTypeCategoryWriteService writeService,
    ILogger<DeleteJobTypeCategoryCommandHandler> logger)
    : IRequestHandler<DeleteJobTypeCategoryCommand, ApiResponse<JobTypeCategoryDetailDto>>
{
    public async Task<ApiResponse<JobTypeCategoryDetailDto>> Handle(
        DeleteJobTypeCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted job type category {Id}", result.Id);
            return ApiResponse<JobTypeCategoryDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete job type category {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<JobTypeCategoryDetailDto>(ex);
        }
    }
}
