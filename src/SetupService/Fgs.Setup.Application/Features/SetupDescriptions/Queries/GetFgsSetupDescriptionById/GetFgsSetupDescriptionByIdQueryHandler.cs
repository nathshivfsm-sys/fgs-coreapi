using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupDescriptions;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Queries.GetFgsSetupDescriptionById;

public sealed class GetFgsSetupDescriptionByIdQueryHandler(IFgsSetupDescriptionReadRepository readRepository)
    : IRequestHandler<GetFgsSetupDescriptionByIdQuery, ApiResponse<FgsSetupDescriptionDetailDto>>
{
    public async Task<ApiResponse<FgsSetupDescriptionDetailDto>> Handle(
        GetFgsSetupDescriptionByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsSetupDescriptionDetailDto>.Fail(
                    [$"Setup Description '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSetupDescriptionDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSetupDescriptionDetailDto>(ex);
        }
    }
}
