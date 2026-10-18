using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Commands.UpsertCrmDefaultCustomer;

public sealed class UpsertCrmDefaultCustomerCommandHandler(ICrmCustomerWriteService writeService)
    : IRequestHandler<UpsertCrmDefaultCustomerCommand, ApiResponse<CrmDefaultCustomerDto>>
{
    public async Task<ApiResponse<CrmDefaultCustomerDto>> Handle(
        UpsertCrmDefaultCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpsertDefaultCustomerAsync(request.Dto, cancellationToken);
        return ApiResponse<CrmDefaultCustomerDto>.Ok(result);
    }
}
