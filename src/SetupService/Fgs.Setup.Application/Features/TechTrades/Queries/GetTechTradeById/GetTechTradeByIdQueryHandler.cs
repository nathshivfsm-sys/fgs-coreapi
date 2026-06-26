using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TechTrades.Queries.GetTechTradeById;

public sealed class GetTechTradeByIdQueryHandler(ITechTradeReadRepository readRepository)
    : IRequestHandler<GetTechTradeByIdQuery, ApiResponse<TechTradeDetailDto>>
{
    public async Task<ApiResponse<TechTradeDetailDto>> Handle(
        GetTechTradeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<TechTradeDetailDto>.Fail(
                [$"Tech trade '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<TechTradeDetailDto>.Ok(result);
    }
}
