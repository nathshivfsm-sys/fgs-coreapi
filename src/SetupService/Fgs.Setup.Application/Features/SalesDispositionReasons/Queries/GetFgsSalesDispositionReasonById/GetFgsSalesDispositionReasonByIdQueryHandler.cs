using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Queries.GetFgsSalesDispositionReasonById;

public sealed class GetFgsSalesDispositionReasonByIdQueryHandler(IFgsSalesDispositionReasonReadRepository readRepository)
    : IRequestHandler<GetFgsSalesDispositionReasonByIdQuery, ApiResponse<FgsSalesDispositionReasonDetailDto>>
{
    public async Task<ApiResponse<FgsSalesDispositionReasonDetailDto>> Handle(
        GetFgsSalesDispositionReasonByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsSalesDispositionReasonDetailDto>.Fail(
                    [$"Sales Disposition Reason '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSalesDispositionReasonDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSalesDispositionReasonDetailDto>(ex);
        }
    }
}
