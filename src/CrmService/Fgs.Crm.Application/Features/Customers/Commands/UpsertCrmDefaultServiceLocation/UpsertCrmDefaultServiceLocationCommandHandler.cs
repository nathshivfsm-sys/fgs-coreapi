using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Commands.UpsertCrmDefaultServiceLocation;

public sealed class UpsertCrmDefaultServiceLocationCommandHandler(ICrmCustomerWriteService writeService)
    : IRequestHandler<UpsertCrmDefaultServiceLocationCommand, ApiResponse<CrmDefaultServiceLocationDto>>
{
    public async Task<ApiResponse<CrmDefaultServiceLocationDto>> Handle(
        UpsertCrmDefaultServiceLocationCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpsertDefaultServiceLocationAsync(request.Dto, cancellationToken);
        return ApiResponse<CrmDefaultServiceLocationDto>.Ok(result);
    }
}
