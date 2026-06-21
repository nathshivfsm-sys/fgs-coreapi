using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Queries.LookupFgsBusinessTypes;

public sealed class LookupFgsBusinessTypesQueryHandler(IFgsBusinessTypeReadRepository readRepository)
    : IRequestHandler<LookupFgsBusinessTypesQuery, ApiResponse<IReadOnlyList<FgsBusinessTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsBusinessTypeLookupDto>>> Handle(
        LookupFgsBusinessTypesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsBusinessTypeLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsBusinessTypeLookupDto>>(ex);
        }
    }
}
