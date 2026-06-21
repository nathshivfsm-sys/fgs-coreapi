using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.GetFgsSetupTaxAuthorityById;

public sealed class GetFgsSetupTaxAuthorityByIdQueryHandler(IFgsSetupTaxAuthorityReadRepository readRepository)
    : IRequestHandler<GetFgsSetupTaxAuthorityByIdQuery, ApiResponse<FgsSetupTaxAuthorityDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxAuthorityDetailDto>> Handle(
        GetFgsSetupTaxAuthorityByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsSetupTaxAuthorityDetailDto>.Fail(
                    [$"Tax Authority '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSetupTaxAuthorityDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSetupTaxAuthorityDetailDto>(ex);
        }
    }
}
