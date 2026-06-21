using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxDetails;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Queries.GetFgsSetupTaxDetailById;

public sealed class GetFgsSetupTaxDetailByIdQueryHandler(IFgsSetupTaxDetailReadRepository readRepository)
    : IRequestHandler<GetFgsSetupTaxDetailByIdQuery, ApiResponse<FgsSetupTaxDetailDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxDetailDetailDto>> Handle(
        GetFgsSetupTaxDetailByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsSetupTaxDetailDetailDto>.Fail(
                    [$"Tax Detail '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSetupTaxDetailDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSetupTaxDetailDetailDto>(ex);
        }
    }
}
