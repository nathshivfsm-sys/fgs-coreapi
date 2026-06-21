using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.PatchFgsSetupTaxAuthority;

public sealed class PatchFgsSetupTaxAuthorityCommandHandler(
    IFgsSetupTaxAuthorityWriteService writeService,
    ILogger<PatchFgsSetupTaxAuthorityCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupTaxAuthorityCommand, ApiResponse<FgsSetupTaxAuthorityDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxAuthorityDetailDto>> Handle(
        PatchFgsSetupTaxAuthorityCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd tax authority {Id}", result.Id);
            return ApiResponse<FgsSetupTaxAuthorityDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch tax authority {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupTaxAuthorityDetailDto>(ex);
        }
    }
}
