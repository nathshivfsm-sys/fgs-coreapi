using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadStatuses;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadStatuses.Queries.LookupLeadStatuses;

public sealed class LookupLeadStatusesQueryHandler(ILeadStatusReadRepository readRepository)
    : IRequestHandler<LookupLeadStatusesQuery, ApiResponse<IReadOnlyList<LeadStatusLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<LeadStatusLookupDto>>> Handle(
        LookupLeadStatusesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<LeadStatusLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<LeadStatusLookupDto>>(ex);
        }
    }
}
