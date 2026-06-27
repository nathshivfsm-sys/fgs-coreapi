using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.Setup.Application.Features.BusinessTypes.Commands.AddCompanyBusinessTypes;

public sealed record AddCompanyBusinessTypesCommand(
    long TenantId,
    long CompanyId,
    AddCompanyBusinessTypesRequest Request,
    string? InternalServiceKey = null,
    string? RequestingServiceName = null)
    : IRequest<ApiResponse<object>>;
