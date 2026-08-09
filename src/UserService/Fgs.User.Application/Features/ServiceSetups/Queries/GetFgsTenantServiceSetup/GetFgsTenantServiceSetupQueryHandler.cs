using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ServiceSetups;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceSetups.Queries.GetFgsTenantServiceSetup;

public sealed class GetFgsTenantServiceSetupQueryHandler(IFgsTenantServiceSetupReadRepository readRepository)
    : IRequestHandler<GetFgsTenantServiceSetupQuery, ApiResponse<FgsTenantServiceSetupDetailDto>>
{
    public async Task<ApiResponse<FgsTenantServiceSetupDetailDto>> Handle(
        GetFgsTenantServiceSetupQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetCurrentAsync(cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsTenantServiceSetupDetailDto>.Fail(
                ["Service setup was not found for the current company."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsTenantServiceSetupDetailDto>.Ok(result);
    }
}
