using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Queries.LookupResolutionCodes;

public sealed class LookupResolutionCodesQueryHandler(IResolutionCodeReadRepository readRepository)
    : IRequestHandler<LookupResolutionCodesQuery, ApiResponse<IReadOnlyList<ResolutionCodeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<ResolutionCodeLookupDto>>> Handle(
        LookupResolutionCodesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<ResolutionCodeLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<ResolutionCodeLookupDto>>(ex);
        }
    }
}
