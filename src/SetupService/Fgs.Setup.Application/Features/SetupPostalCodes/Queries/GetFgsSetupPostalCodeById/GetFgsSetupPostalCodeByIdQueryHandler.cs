using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPostalCodes;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Queries.GetFgsSetupPostalCodeById;

public sealed class GetFgsSetupPostalCodeByIdQueryHandler(IFgsSetupPostalCodeReadRepository readRepository)
    : IRequestHandler<GetFgsSetupPostalCodeByIdQuery, ApiResponse<FgsSetupPostalCodeDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPostalCodeDetailDto>> Handle(
        GetFgsSetupPostalCodeByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsSetupPostalCodeDetailDto>.Fail(
                    [$"Postal Code '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSetupPostalCodeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSetupPostalCodeDetailDto>(ex);
        }
    }
}
