using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Commands.CreateResolutionCode;

public sealed class CreateResolutionCodeCommandHandler(
    IResolutionCodeWriteService writeService,
    ILogger<CreateResolutionCodeCommandHandler> logger)
    : IRequestHandler<CreateResolutionCodeCommand, ApiResponse<ResolutionCodeDetailDto>>
{
    public async Task<ApiResponse<ResolutionCodeDetailDto>> Handle(
        CreateResolutionCodeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created resolution code {Id} with code {ResolutionCode}", result.Id, result.ResolutionCode);
            return ApiResponse<ResolutionCodeDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create resolution code");
            return CatalogCrudExceptionMapper.MapException<ResolutionCodeDetailDto>(ex);
        }
    }
}
