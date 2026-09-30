using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceSetups.Queries.GetFgsTenantServiceSetupById;

public sealed record GetFgsTenantServiceSetupByIdQuery(long CompanyId)
    : IRequest<ApiResponse<FgsTenantServiceSetupDetailDto>>;
