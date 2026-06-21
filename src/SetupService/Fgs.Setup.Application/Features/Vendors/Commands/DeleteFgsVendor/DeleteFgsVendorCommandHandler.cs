using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Vendors;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Vendors.Commands.DeleteFgsVendor;

public sealed class DeleteFgsVendorCommandHandler(
    IFgsVendorWriteService writeService,
    ILogger<DeleteFgsVendorCommandHandler> logger)
    : IRequestHandler<DeleteFgsVendorCommand, ApiResponse<FgsVendorDetailDto>>
{
    public async Task<ApiResponse<FgsVendorDetailDto>> Handle(
        DeleteFgsVendorCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted vendor {Id}", result.Id);
            return ApiResponse<FgsVendorDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete vendor {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsVendorDetailDto>(ex);
        }
    }
}
