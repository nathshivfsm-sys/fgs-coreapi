using Fgs.Contracts.Api;
using Fgs.Credentials;
using Fgs.Credentials.Options;
using Fgs.Setup.Application.Abstractions.Tenants;
using MediatR;
using Microsoft.Extensions.Options;

namespace Fgs.Setup.Application.Features.BusinessTypes.Commands.AddCompanyBusinessTypes;

public sealed class AddCompanyBusinessTypesCommandHandler(
    ICompanyBusinessTypeService service,
    IOptions<CredentialDistributionOptions> distributionOptions)
    : IRequestHandler<AddCompanyBusinessTypesCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        AddCompanyBusinessTypesCommand request,
        CancellationToken cancellationToken)
    {
        if (!InternalServiceAuthorization.IsAuthorized(
                request.InternalServiceKey,
                distributionOptions.Value))
        {
            return ApiResponse<object>.Fail(["Unauthorized."], ApiStatusCodes.Unauthorized);
        }

        await service.AddCompanyBusinessTypesAsync(
            request.TenantId,
            request.CompanyId,
            request.Request,
            cancellationToken);

        return ApiResponse<object>.Ok(new object());
    }
}
