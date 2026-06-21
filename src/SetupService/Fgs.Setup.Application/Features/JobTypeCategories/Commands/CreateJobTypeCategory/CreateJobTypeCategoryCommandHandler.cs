using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Commands.CreateJobTypeCategory;

public sealed class CreateJobTypeCategoryCommandHandler(
    IJobTypeCategoryWriteService writeService,
    ILogger<CreateJobTypeCategoryCommandHandler> logger)
    : IRequestHandler<CreateJobTypeCategoryCommand, ApiResponse<JobTypeCategoryDetailDto>>
{
    public async Task<ApiResponse<JobTypeCategoryDetailDto>> Handle(
        CreateJobTypeCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created job type category {Id} with code {CategoryCode}", result.Id, result.CategoryCode);
            return ApiResponse<JobTypeCategoryDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create job type category");
            return CatalogCrudExceptionMapper.MapException<JobTypeCategoryDetailDto>(ex);
        }
    }
}
