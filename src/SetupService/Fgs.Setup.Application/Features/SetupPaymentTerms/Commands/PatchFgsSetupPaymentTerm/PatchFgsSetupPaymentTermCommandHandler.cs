using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.PatchFgsSetupPaymentTerm;

public sealed class PatchFgsSetupPaymentTermCommandHandler(
    IFgsSetupPaymentTermWriteService writeService,
    ILogger<PatchFgsSetupPaymentTermCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupPaymentTermCommand, ApiResponse<FgsSetupPaymentTermDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPaymentTermDetailDto>> Handle(
        PatchFgsSetupPaymentTermCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd payment term {Id}", result.Id);
            return ApiResponse<FgsSetupPaymentTermDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch payment term {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupPaymentTermDetailDto>(ex);
        }
    }
}
