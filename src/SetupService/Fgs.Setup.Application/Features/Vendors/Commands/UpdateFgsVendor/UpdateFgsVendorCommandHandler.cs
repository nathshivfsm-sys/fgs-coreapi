using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Vendors;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Vendors.Commands.UpdateFgsVendor;

public sealed class UpdateFgsVendorCommandHandler(
    IFgsVendorWriteService writeService,
    ILogger<UpdateFgsVendorCommandHandler> logger)
    : IRequestHandler<UpdateFgsVendorCommand, ApiResponse<FgsVendorDetailDto>>
{
    public async Task<ApiResponse<FgsVendorDetailDto>> Handle(
        UpdateFgsVendorCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated vendor {Id}", result.Id);
            return ApiResponse<FgsVendorDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update vendor {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsVendorDetailDto>(ex);
        }
    }
}
