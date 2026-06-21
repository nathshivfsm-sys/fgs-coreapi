using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Tags;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Tags.Commands.CreateFgsTag;

public sealed class CreateFgsTagCommandHandler(
    IFgsTagWriteService writeService,
    ILogger<CreateFgsTagCommandHandler> logger)
    : IRequestHandler<CreateFgsTagCommand, ApiResponse<FgsTagDetailDto>>
{
    public async Task<ApiResponse<FgsTagDetailDto>> Handle(
        CreateFgsTagCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created tag {Id} with code {TagCode}", result.Id, result.TagCode);
            return ApiResponse<FgsTagDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create tag");
            return CatalogCrudExceptionMapper.MapException<FgsTagDetailDto>(ex);
        }
    }
}
