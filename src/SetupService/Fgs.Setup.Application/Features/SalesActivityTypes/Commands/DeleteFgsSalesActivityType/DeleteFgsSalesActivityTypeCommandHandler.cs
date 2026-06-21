using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityTypes;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Commands.DeleteFgsSalesActivityType;

public sealed class DeleteFgsSalesActivityTypeCommandHandler(
    IFgsSalesActivityTypeWriteService writeService,
    ILogger<DeleteFgsSalesActivityTypeCommandHandler> logger)
    : IRequestHandler<DeleteFgsSalesActivityTypeCommand, ApiResponse<FgsSalesActivityTypeDetailDto>>
{
    public async Task<ApiResponse<FgsSalesActivityTypeDetailDto>> Handle(
        DeleteFgsSalesActivityTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted sales activity type {Id}", result.Id);
            return ApiResponse<FgsSalesActivityTypeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete sales activity type {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSalesActivityTypeDetailDto>(ex);
        }
    }
}
