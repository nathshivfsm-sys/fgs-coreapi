using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.BillingCategories.Commands.PatchBillingCategory;

public sealed class PatchBillingCategoryCommandHandler(
    IBillingCategoryWriteService writeService,
    ILogger<PatchBillingCategoryCommandHandler> logger)
    : IRequestHandler<PatchBillingCategoryCommand, ApiResponse<BillingCategoryDetailDto>>
{
    public async Task<ApiResponse<BillingCategoryDetailDto>> Handle(
        PatchBillingCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd billing category {Id}", result.Id);
            return ApiResponse<BillingCategoryDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch billing category {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<BillingCategoryDetailDto>(ex);
        }
    }
}
