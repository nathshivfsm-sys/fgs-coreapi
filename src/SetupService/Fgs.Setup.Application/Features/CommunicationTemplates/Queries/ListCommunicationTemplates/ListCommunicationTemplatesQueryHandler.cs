using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Queries.ListCommunicationTemplates;

public sealed class ListCommunicationTemplatesQueryHandler(IFgsSetupCommunicationTemplateReadRepository readRepository)
    : IRequestHandler<ListCommunicationTemplatesQuery, ApiResponse<PagedResult<FgsSetupCommunicationTemplateSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupCommunicationTemplateSummaryDto>>> Handle(
        ListCommunicationTemplatesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
            return ApiResponse<PagedResult<FgsSetupCommunicationTemplateSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupCommunicationTemplateSummaryDto>>(ex);
        }
    }
}
