using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.GetCrmDefaultServiceLocation;

public sealed record GetCrmDefaultServiceLocationQuery
    : IRequest<ApiResponse<CrmDefaultServiceLocationDto>>;
