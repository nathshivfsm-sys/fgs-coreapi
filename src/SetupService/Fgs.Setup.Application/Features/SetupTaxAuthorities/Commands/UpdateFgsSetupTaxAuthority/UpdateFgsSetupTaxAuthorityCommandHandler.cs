using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.UpdateFgsSetupTaxAuthority;

public sealed class UpdateFgsSetupTaxAuthorityCommandHandler(
    IFgsSetupTaxAuthorityWriteService writeService,
    ILogger<UpdateFgsSetupTaxAuthorityCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupTaxAuthorityCommand, ApiResponse<FgsSetupTaxAuthorityDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxAuthorityDetailDto>> Handle(
        UpdateFgsSetupTaxAuthorityCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated tax authority {Id}", result.Id);
            return ApiResponse<FgsSetupTaxAuthorityDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update tax authority {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupTaxAuthorityDetailDto>(ex);
        }
    }
}
