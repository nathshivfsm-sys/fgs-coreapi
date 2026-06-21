using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Commands.CreateFgsSalesDispositionReason;

public sealed class CreateFgsSalesDispositionReasonCommandHandler(
    IFgsSalesDispositionReasonWriteService writeService,
    ILogger<CreateFgsSalesDispositionReasonCommandHandler> logger)
    : IRequestHandler<CreateFgsSalesDispositionReasonCommand, ApiResponse<FgsSalesDispositionReasonDetailDto>>
{
    public async Task<ApiResponse<FgsSalesDispositionReasonDetailDto>> Handle(
        CreateFgsSalesDispositionReasonCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created sales disposition reason {Id} with code {DispositionReasonCode}", result.Id, result.DispositionReasonCode);
            return ApiResponse<FgsSalesDispositionReasonDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create sales disposition reason");
            return CatalogCrudExceptionMapper.MapException<FgsSalesDispositionReasonDetailDto>(ex);
        }
    }
}
