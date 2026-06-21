using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupZones;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupZones.Queries.GetFgsSetupZoneById;

public sealed class GetFgsSetupZoneByIdQueryHandler(IFgsSetupZoneReadRepository readRepository)
    : IRequestHandler<GetFgsSetupZoneByIdQuery, ApiResponse<FgsSetupZoneDetailDto>>
{
    public async Task<ApiResponse<FgsSetupZoneDetailDto>> Handle(
        GetFgsSetupZoneByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsSetupZoneDetailDto>.Fail(
                    [$"Zone '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSetupZoneDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSetupZoneDetailDto>(ex);
        }
    }
}
