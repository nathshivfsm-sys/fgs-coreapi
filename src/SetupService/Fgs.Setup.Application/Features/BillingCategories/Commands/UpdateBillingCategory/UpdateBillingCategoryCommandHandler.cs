using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.BillingCategories.Commands.UpdateBillingCategory;

public sealed class UpdateBillingCategoryCommandHandler(
    IBillingCategoryWriteService writeService,
    ILogger<UpdateBillingCategoryCommandHandler> logger)
    : IRequestHandler<UpdateBillingCategoryCommand, ApiResponse<BillingCategoryDetailDto>>
{
    public async Task<ApiResponse<BillingCategoryDetailDto>> Handle(
        UpdateBillingCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated billing category {Id}", result.Id);
            return ApiResponse<BillingCategoryDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update billing category {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<BillingCategoryDetailDto>(ex);
        }
    }
}
