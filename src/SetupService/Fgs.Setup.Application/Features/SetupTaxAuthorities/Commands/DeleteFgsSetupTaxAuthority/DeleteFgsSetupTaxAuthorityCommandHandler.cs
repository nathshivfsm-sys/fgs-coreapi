using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.DeleteFgsSetupTaxAuthority;

public sealed class DeleteFgsSetupTaxAuthorityCommandHandler(
    IFgsSetupTaxAuthorityWriteService writeService,
    ILogger<DeleteFgsSetupTaxAuthorityCommandHandler> logger)
    : IRequestHandler<DeleteFgsSetupTaxAuthorityCommand, ApiResponse<FgsSetupTaxAuthorityDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxAuthorityDetailDto>> Handle(
        DeleteFgsSetupTaxAuthorityCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted tax authority {Id}", result.Id);
            return ApiResponse<FgsSetupTaxAuthorityDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete tax authority {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupTaxAuthorityDetailDto>(ex);
        }
    }
}
