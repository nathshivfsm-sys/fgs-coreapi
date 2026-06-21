using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxes.Queries.GetFgsSetupTaxById;

public sealed class GetFgsSetupTaxByIdQueryHandler(IFgsSetupTaxReadRepository readRepository)
    : IRequestHandler<GetFgsSetupTaxByIdQuery, ApiResponse<FgsSetupTaxDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxDetailDto>> Handle(
        GetFgsSetupTaxByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsSetupTaxDetailDto>.Fail(
                    [$"Tax '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSetupTaxDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSetupTaxDetailDto>(ex);
        }
    }
}
