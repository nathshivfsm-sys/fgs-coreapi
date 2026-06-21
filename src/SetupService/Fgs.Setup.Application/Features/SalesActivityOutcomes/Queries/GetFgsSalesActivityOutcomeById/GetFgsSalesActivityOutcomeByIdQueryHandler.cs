using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Queries.GetFgsSalesActivityOutcomeById;

public sealed class GetFgsSalesActivityOutcomeByIdQueryHandler(IFgsSalesActivityOutcomeReadRepository readRepository)
    : IRequestHandler<GetFgsSalesActivityOutcomeByIdQuery, ApiResponse<FgsSalesActivityOutcomeDetailDto>>
{
    public async Task<ApiResponse<FgsSalesActivityOutcomeDetailDto>> Handle(
        GetFgsSalesActivityOutcomeByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsSalesActivityOutcomeDetailDto>.Fail(
                    [$"Sales Activity Outcome '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSalesActivityOutcomeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSalesActivityOutcomeDetailDto>(ex);
        }
    }
}
