using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeSubCategories;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.CreateJobTypeSubCategory;

public sealed class CreateJobTypeSubCategoryCommandHandler(
    IJobTypeSubCategoryWriteService writeService,
    ILogger<CreateJobTypeSubCategoryCommandHandler> logger)
    : IRequestHandler<CreateJobTypeSubCategoryCommand, ApiResponse<JobTypeSubCategoryDetailDto>>
{
    public async Task<ApiResponse<JobTypeSubCategoryDetailDto>> Handle(
        CreateJobTypeSubCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created job type subcategory {Id} with code {SubCategoryCode}", result.Id, result.SubCategoryCode);
            return ApiResponse<JobTypeSubCategoryDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create job type subcategory");
            return CatalogCrudExceptionMapper.MapException<JobTypeSubCategoryDetailDto>(ex);
        }
    }
}
