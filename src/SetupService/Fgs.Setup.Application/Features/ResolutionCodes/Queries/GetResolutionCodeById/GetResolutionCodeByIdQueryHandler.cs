using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Queries.GetResolutionCodeById;

public sealed class GetResolutionCodeByIdQueryHandler(IResolutionCodeReadRepository readRepository)
    : IRequestHandler<GetResolutionCodeByIdQuery, ApiResponse<ResolutionCodeDetailDto>>
{
    public async Task<ApiResponse<ResolutionCodeDetailDto>> Handle(
        GetResolutionCodeByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<ResolutionCodeDetailDto>.Fail(
                    [$"Resolution Code '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<ResolutionCodeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<ResolutionCodeDetailDto>(ex);
        }
    }
}
