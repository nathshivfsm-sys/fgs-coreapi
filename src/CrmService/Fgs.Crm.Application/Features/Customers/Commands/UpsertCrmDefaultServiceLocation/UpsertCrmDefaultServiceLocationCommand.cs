using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Commands.UpsertCrmDefaultServiceLocation;

public sealed record UpsertCrmDefaultServiceLocationCommand(CrmDefaultServiceLocationUpsertDto Dto)
    : IRequest<ApiResponse<CrmDefaultServiceLocationDto>>;
