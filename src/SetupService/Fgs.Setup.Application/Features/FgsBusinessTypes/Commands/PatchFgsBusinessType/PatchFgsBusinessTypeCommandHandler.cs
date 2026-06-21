using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Commands.PatchFgsBusinessType;

public sealed class PatchFgsBusinessTypeCommandHandler(
    IFgsBusinessTypeWriteService writeService,
    ILogger<PatchFgsBusinessTypeCommandHandler> logger)
    : IRequestHandler<PatchFgsBusinessTypeCommand, ApiResponse<FgsBusinessTypeDetailDto>>
{
    public async Task<ApiResponse<FgsBusinessTypeDetailDto>> Handle(
        PatchFgsBusinessTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd business type {Id}", result.Id);
            return ApiResponse<FgsBusinessTypeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch business type {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsBusinessTypeDetailDto>(ex);
        }
    }
}
