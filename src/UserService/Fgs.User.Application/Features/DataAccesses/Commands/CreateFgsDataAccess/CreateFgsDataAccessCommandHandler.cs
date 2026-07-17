using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.DataAccesses;
using Fgs.User.Application.Features.DataAccesses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.DataAccesses.Commands.CreateFgsDataAccess;

public sealed class CreateFgsDataAccessCommandHandler(
    IFgsDataAccessWriteService writeService,
    ILogger<CreateFgsDataAccessCommandHandler> logger)
    : IRequestHandler<CreateFgsDataAccessCommand, ApiResponse<FgsDataAccessDetailDto>>
{
    public async Task<ApiResponse<FgsDataAccessDetailDto>> Handle(
        CreateFgsDataAccessCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created data access {DataAccessId} with code {DataAccessCode}",
            result.Id,
            result.DataAccessCode);
        return ApiResponse<FgsDataAccessDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
