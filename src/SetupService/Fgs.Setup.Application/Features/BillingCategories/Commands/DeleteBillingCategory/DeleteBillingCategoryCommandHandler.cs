using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.BillingCategories.Commands.DeleteBillingCategory;

public sealed class DeleteBillingCategoryCommandHandler(
    IBillingCategoryWriteService writeService,
    ILogger<DeleteBillingCategoryCommandHandler> logger)
    : IRequestHandler<DeleteBillingCategoryCommand, ApiResponse<BillingCategoryDetailDto>>
{
    public async Task<ApiResponse<BillingCategoryDetailDto>> Handle(
        DeleteBillingCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted billing category {Id}", result.Id);
            return ApiResponse<BillingCategoryDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete billing category {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<BillingCategoryDetailDto>(ex);
        }
    }
}
