using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Queries.ListTitlesOfCourtesy;

public sealed class ListTitlesOfCourtesyQueryHandler(ITitleOfCourtesyReadRepository readRepository)
    : IRequestHandler<ListTitlesOfCourtesyQuery, ApiResponse<PagedResult<TitleOfCourtesySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<TitleOfCourtesySummaryDto>>> Handle(
        ListTitlesOfCourtesyQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<TitleOfCourtesySummaryDto>>.Ok(result);
    }
}
