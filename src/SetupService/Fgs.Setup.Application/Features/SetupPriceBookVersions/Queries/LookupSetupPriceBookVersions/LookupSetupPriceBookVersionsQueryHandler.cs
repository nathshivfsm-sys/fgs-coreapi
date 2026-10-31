using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.SetupPriceBookVersions;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPriceBookVersions.Queries.LookupSetupPriceBookVersions;

public sealed class LookupSetupPriceBookVersionsQueryHandler(
    IFgsSetupPriceBookVersionReadRepository readRepository)
    : IRequestHandler<LookupSetupPriceBookVersionsQuery, ApiResponse<IReadOnlyList<FgsSetupPriceBookVersionLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupPriceBookVersionLookupDto>>> Handle(
        LookupSetupPriceBookVersionsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsSetupPriceBookVersionLookupDto>>.Ok(result);
    }
}
