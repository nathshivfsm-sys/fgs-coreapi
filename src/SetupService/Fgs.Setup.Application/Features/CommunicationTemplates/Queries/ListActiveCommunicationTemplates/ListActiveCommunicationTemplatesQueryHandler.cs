using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Queries.ListActiveCommunicationTemplates;

public sealed class ListActiveCommunicationTemplatesQueryHandler(IFgsSetupCommunicationTemplateReadRepository readRepository)
    : IRequestHandler<ListActiveCommunicationTemplatesQuery, ApiResponse<PagedResult<FgsSetupCommunicationTemplateSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupCommunicationTemplateSummaryDto>>> Handle(
        ListActiveCommunicationTemplatesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new SetupListQuery(
                request.Page,
                request.PageSize,
                request.SortBy,
                request.SortDirection,
                request.Search,
                IsActive: true);

            var result = await readRepository.ListAsync(
                query,
                request.Filters ?? new FgsSetupCommunicationTemplateListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSetupCommunicationTemplateSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupCommunicationTemplateSummaryDto>>(ex);
        }
    }
}
