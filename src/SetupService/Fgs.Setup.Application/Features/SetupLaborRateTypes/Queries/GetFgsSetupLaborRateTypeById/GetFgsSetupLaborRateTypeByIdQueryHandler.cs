using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Queries.GetFgsSetupLaborRateTypeById;

public sealed class GetFgsSetupLaborRateTypeByIdQueryHandler(IFgsSetupLaborRateTypeReadRepository readRepository)
    : IRequestHandler<GetFgsSetupLaborRateTypeByIdQuery, ApiResponse<FgsSetupLaborRateTypeDetailDto>>
{
    public async Task<ApiResponse<FgsSetupLaborRateTypeDetailDto>> Handle(
        GetFgsSetupLaborRateTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsSetupLaborRateTypeDetailDto>.Fail(
                    [$"Labor Rate Type '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSetupLaborRateTypeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSetupLaborRateTypeDetailDto>(ex);
        }
    }
}
