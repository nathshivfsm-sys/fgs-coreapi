using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupDescriptions;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Queries.LookupSetupDescriptions;

public sealed class LookupSetupDescriptionsQueryHandler(IFgsSetupDescriptionReadRepository readRepository)
    : IRequestHandler<LookupSetupDescriptionsQuery, ApiResponse<IReadOnlyList<FgsSetupDescriptionLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupDescriptionLookupDto>>> Handle(
        LookupSetupDescriptionsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSetupDescriptionLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSetupDescriptionLookupDto>>(ex);
        }
    }
}
