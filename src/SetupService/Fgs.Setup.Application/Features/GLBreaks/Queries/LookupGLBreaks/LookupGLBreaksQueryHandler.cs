using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.GLBreaks;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GLBreaks.Queries.LookupGLBreaks;

public sealed class LookupGLBreaksQueryHandler(IGLBreakReadRepository readRepository)
    : IRequestHandler<LookupGLBreaksQuery, ApiResponse<IReadOnlyList<GLBreakLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GLBreakLookupDto>>> Handle(
        LookupGLBreaksQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<GLBreakLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<GLBreakLookupDto>>(ex);
        }
    }
}
