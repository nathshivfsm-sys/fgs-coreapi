using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Commands.PatchCrmCustomer;

public sealed record PatchCrmCustomerCommand(long Id, CrmCustomerPatchDto Dto)
    : IRequest<ApiResponse<CrmCustomerDetailDto>>;
