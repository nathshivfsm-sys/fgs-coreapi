using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.GLBreaks;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.GLBreaks.Commands.CreateGLBreak;

public sealed class CreateGLBreakCommandHandler(
    IGLBreakWriteService writeService,
    ILogger<CreateGLBreakCommandHandler> logger)
    : IRequestHandler<CreateGLBreakCommand, ApiResponse<GLBreakDetailDto>>
{
    public async Task<ApiResponse<GLBreakDetailDto>> Handle(
        CreateGLBreakCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation(
                "Created GL break {GLBreakId} with code {Code} for tenant {TenantId} company {CompanyId}",
                result.Id,
                result.Code,
                result.TenantId,
                result.CompanyId);

            return ApiResponse<GLBreakDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create GL break with code {Code}", request.Dto.Code);
            return CatalogCrudExceptionMapper.MapException<GLBreakDetailDto>(ex);
        }
    }
}
