using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.GLBreaks;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GLBreaks.Queries.GetGLBreakById;

public sealed class GetGLBreakByIdQueryHandler(IGLBreakReadRepository readRepository)
    : IRequestHandler<GetGLBreakByIdQuery, ApiResponse<GLBreakDetailDto>>
{
    public async Task<ApiResponse<GLBreakDetailDto>> Handle(
        GetGLBreakByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        return result is null
                ? ApiResponse<GLBreakDetailDto>.Fail(
                    [$"GL break '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound)
                : ApiResponse<GLBreakDetailDto>.Ok(result);
    }
}
