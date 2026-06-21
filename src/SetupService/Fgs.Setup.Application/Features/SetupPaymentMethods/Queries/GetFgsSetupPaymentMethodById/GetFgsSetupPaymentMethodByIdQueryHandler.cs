using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Queries.GetFgsSetupPaymentMethodById;

public sealed class GetFgsSetupPaymentMethodByIdQueryHandler(IFgsSetupPaymentMethodReadRepository readRepository)
    : IRequestHandler<GetFgsSetupPaymentMethodByIdQuery, ApiResponse<FgsSetupPaymentMethodDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPaymentMethodDetailDto>> Handle(
        GetFgsSetupPaymentMethodByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsSetupPaymentMethodDetailDto>.Fail(
                    [$"Payment Method '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSetupPaymentMethodDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSetupPaymentMethodDetailDto>(ex);
        }
    }
}
