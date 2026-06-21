using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.DeleteFgsSetupPaymentTerm;

public sealed class DeleteFgsSetupPaymentTermCommandHandler(
    IFgsSetupPaymentTermWriteService writeService,
    ILogger<DeleteFgsSetupPaymentTermCommandHandler> logger)
    : IRequestHandler<DeleteFgsSetupPaymentTermCommand, ApiResponse<FgsSetupPaymentTermDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPaymentTermDetailDto>> Handle(
        DeleteFgsSetupPaymentTermCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted payment term {Id}", result.Id);
            return ApiResponse<FgsSetupPaymentTermDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete payment term {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupPaymentTermDetailDto>(ex);
        }
    }
}
