using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.CreateTitleOfCourtesy;

public sealed class CreateTitleOfCourtesyCommandHandler(
    ITitleOfCourtesyWriteService writeService,
    ILogger<CreateTitleOfCourtesyCommandHandler> logger)
    : IRequestHandler<CreateTitleOfCourtesyCommand, ApiResponse<TitleOfCourtesyDetailDto>>
{
    public async Task<ApiResponse<TitleOfCourtesyDetailDto>> Handle(
        CreateTitleOfCourtesyCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation(
                "Created title of courtesy {TitleOfCourtesyId} with code {Code} for tenant {TenantId} company {CompanyId}",
                result.Id,
                result.Code,
                result.TenantId,
                result.CompanyId);

            return ApiResponse<TitleOfCourtesyDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create title of courtesy with code {Code}", request.Dto.Code);
            return CatalogCrudExceptionMapper.MapException<TitleOfCourtesyDetailDto>(ex);
        }
    }
}
