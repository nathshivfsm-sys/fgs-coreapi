using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Commands.UpdateJobTypeCategory;

public sealed class UpdateJobTypeCategoryCommandHandler(
    IJobTypeCategoryWriteService writeService,
    ILogger<UpdateJobTypeCategoryCommandHandler> logger)
    : IRequestHandler<UpdateJobTypeCategoryCommand, ApiResponse<JobTypeCategoryDetailDto>>
{
    public async Task<ApiResponse<JobTypeCategoryDetailDto>> Handle(
        UpdateJobTypeCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated job type category {Id}", result.Id);
            return ApiResponse<JobTypeCategoryDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update job type category {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<JobTypeCategoryDetailDto>(ex);
        }
    }
}
