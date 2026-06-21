using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Vendors;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Vendors.Commands.CreateFgsVendor;

public sealed class CreateFgsVendorCommandHandler(
    IFgsVendorWriteService writeService,
    ILogger<CreateFgsVendorCommandHandler> logger)
    : IRequestHandler<CreateFgsVendorCommand, ApiResponse<FgsVendorDetailDto>>
{
    public async Task<ApiResponse<FgsVendorDetailDto>> Handle(
        CreateFgsVendorCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created vendor {Id} with code {VendorCode}", result.Id, result.VendorCode);
            return ApiResponse<FgsVendorDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create vendor");
            return CatalogCrudExceptionMapper.MapException<FgsVendorDetailDto>(ex);
        }
    }
}
