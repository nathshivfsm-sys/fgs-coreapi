using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Queries.GetFgsBusinessTypeById;

public sealed class GetFgsBusinessTypeByIdQueryHandler(IFgsBusinessTypeReadRepository readRepository)
    : IRequestHandler<GetFgsBusinessTypeByIdQuery, ApiResponse<FgsBusinessTypeDetailDto>>
{
    public async Task<ApiResponse<FgsBusinessTypeDetailDto>> Handle(
        GetFgsBusinessTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsBusinessTypeDetailDto>.Fail(
                    [$"Business Type '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsBusinessTypeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsBusinessTypeDetailDto>(ex);
        }
    }
}
