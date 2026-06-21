using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.LookupLeadDisqualificationReasons;

public sealed class LookupLeadDisqualificationReasonsQueryHandler(ILeadDisqualificationReasonReadRepository readRepository)
    : IRequestHandler<LookupLeadDisqualificationReasonsQuery, ApiResponse<IReadOnlyList<LeadDisqualificationReasonLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<LeadDisqualificationReasonLookupDto>>> Handle(
        LookupLeadDisqualificationReasonsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<LeadDisqualificationReasonLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<LeadDisqualificationReasonLookupDto>>(ex);
        }
    }
}
