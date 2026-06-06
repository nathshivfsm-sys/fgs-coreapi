using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.Tenants;
using MediatR;

namespace Fgs.Setup.Application.Features.BusinessTypes.Commands.AddCompanyBusinessTypes;

public sealed class AddCompanyBusinessTypesCommandHandler(ICompanyBusinessTypeService service)
    : IRequestHandler<AddCompanyBusinessTypesCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        AddCompanyBusinessTypesCommand request,
        CancellationToken cancellationToken)
    {
        await service.AddCompanyBusinessTypesAsync(
            request.TenantId,
            request.CompanyId,
            request.Request,
            cancellationToken);

        return ApiResponse<object>.Ok(new object());
    }
}
