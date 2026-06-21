using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityTypes;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Queries.GetFgsSalesActivityTypeById;

public sealed class GetFgsSalesActivityTypeByIdQueryHandler(IFgsSalesActivityTypeReadRepository readRepository)
    : IRequestHandler<GetFgsSalesActivityTypeByIdQuery, ApiResponse<FgsSalesActivityTypeDetailDto>>
{
    public async Task<ApiResponse<FgsSalesActivityTypeDetailDto>> Handle(
        GetFgsSalesActivityTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsSalesActivityTypeDetailDto>.Fail(
                    [$"Sales Activity Type '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSalesActivityTypeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSalesActivityTypeDetailDto>(ex);
        }
    }
}
