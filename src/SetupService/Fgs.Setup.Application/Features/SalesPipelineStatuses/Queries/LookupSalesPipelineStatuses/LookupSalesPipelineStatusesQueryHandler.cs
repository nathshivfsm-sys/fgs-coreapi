using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Queries.LookupSalesPipelineStatuses;

public sealed class LookupSalesPipelineStatusesQueryHandler(IFgsSalesPipelineStatusReadRepository readRepository)
    : IRequestHandler<LookupSalesPipelineStatusesQuery, ApiResponse<IReadOnlyList<FgsSalesPipelineStatusLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSalesPipelineStatusLookupDto>>> Handle(
        LookupSalesPipelineStatusesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSalesPipelineStatusLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSalesPipelineStatusLookupDto>>(ex);
        }
    }
}
