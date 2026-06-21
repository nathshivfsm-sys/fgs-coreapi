using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Queries.LookupTitlesOfCourtesy;

public sealed class LookupTitlesOfCourtesyQueryHandler(ITitleOfCourtesyReadRepository readRepository)
    : IRequestHandler<LookupTitlesOfCourtesyQuery, ApiResponse<IReadOnlyList<TitleOfCourtesyLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<TitleOfCourtesyLookupDto>>> Handle(
        LookupTitlesOfCourtesyQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<TitleOfCourtesyLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<TitleOfCourtesyLookupDto>>(ex);
        }
    }
}
