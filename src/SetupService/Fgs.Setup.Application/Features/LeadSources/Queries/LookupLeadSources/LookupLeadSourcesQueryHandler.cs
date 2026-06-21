using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadSources;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadSources.Queries.LookupLeadSources;

public sealed class LookupLeadSourcesQueryHandler(ILeadSourceReadRepository readRepository)
    : IRequestHandler<LookupLeadSourcesQuery, ApiResponse<IReadOnlyList<LeadSourceLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<LeadSourceLookupDto>>> Handle(
        LookupLeadSourcesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<LeadSourceLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<LeadSourceLookupDto>>(ex);
        }
    }
}
