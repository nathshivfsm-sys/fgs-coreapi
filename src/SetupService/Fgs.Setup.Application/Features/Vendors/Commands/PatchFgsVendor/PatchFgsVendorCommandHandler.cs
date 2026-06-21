using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Vendors;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Vendors.Commands.PatchFgsVendor;

public sealed class PatchFgsVendorCommandHandler(
    IFgsVendorWriteService writeService,
    ILogger<PatchFgsVendorCommandHandler> logger)
    : IRequestHandler<PatchFgsVendorCommand, ApiResponse<FgsVendorDetailDto>>
{
    public async Task<ApiResponse<FgsVendorDetailDto>> Handle(
        PatchFgsVendorCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd vendor {Id}", result.Id);
            return ApiResponse<FgsVendorDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch vendor {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsVendorDetailDto>(ex);
        }
    }
}
