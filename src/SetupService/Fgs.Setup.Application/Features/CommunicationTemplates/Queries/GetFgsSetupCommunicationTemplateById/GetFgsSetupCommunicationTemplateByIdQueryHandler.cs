using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Queries.GetFgsSetupCommunicationTemplateById;

public sealed class GetFgsSetupCommunicationTemplateByIdQueryHandler(IFgsSetupCommunicationTemplateReadRepository readRepository)
    : IRequestHandler<GetFgsSetupCommunicationTemplateByIdQuery, ApiResponse<FgsSetupCommunicationTemplateDetailDto>>
{
    public async Task<ApiResponse<FgsSetupCommunicationTemplateDetailDto>> Handle(
        GetFgsSetupCommunicationTemplateByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsSetupCommunicationTemplateDetailDto>.Fail(
                    [$"Communication Template '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSetupCommunicationTemplateDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSetupCommunicationTemplateDetailDto>(ex);
        }
    }
}
