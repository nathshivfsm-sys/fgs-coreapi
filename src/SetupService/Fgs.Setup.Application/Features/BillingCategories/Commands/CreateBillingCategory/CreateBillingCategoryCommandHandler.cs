using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.BillingCategories.Commands.CreateBillingCategory;

public sealed class CreateBillingCategoryCommandHandler(
    IBillingCategoryWriteService writeService,
    ILogger<CreateBillingCategoryCommandHandler> logger)
    : IRequestHandler<CreateBillingCategoryCommand, ApiResponse<BillingCategoryDetailDto>>
{
    public async Task<ApiResponse<BillingCategoryDetailDto>> Handle(
        CreateBillingCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created billing category {Id} with code {BillingCategoryType}", result.Id, result.BillingCategoryType);
            return ApiResponse<BillingCategoryDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create billing category");
            return CatalogCrudExceptionMapper.MapException<BillingCategoryDetailDto>(ex);
        }
    }
}
