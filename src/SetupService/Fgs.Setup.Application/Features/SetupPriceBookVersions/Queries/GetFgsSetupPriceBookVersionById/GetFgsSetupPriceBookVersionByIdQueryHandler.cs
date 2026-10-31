using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.SetupPriceBookVersions;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPriceBookVersions.Queries.GetFgsSetupPriceBookVersionById;

public sealed class GetFgsSetupPriceBookVersionByIdQueryHandler(
    IFgsSetupPriceBookVersionReadRepository readRepository)
    : IRequestHandler<GetFgsSetupPriceBookVersionByIdQuery, ApiResponse<FgsSetupPriceBookVersionDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPriceBookVersionDetailDto>> Handle(
        GetFgsSetupPriceBookVersionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsSetupPriceBookVersionDetailDto>.Fail(
                [$"Price book version '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsSetupPriceBookVersionDetailDto>.Ok(result);
    }
}
