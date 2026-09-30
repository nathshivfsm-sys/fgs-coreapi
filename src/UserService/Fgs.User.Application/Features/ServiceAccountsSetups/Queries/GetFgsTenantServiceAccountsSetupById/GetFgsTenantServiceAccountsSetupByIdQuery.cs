using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceAccountsSetups.Queries.GetFgsTenantServiceAccountsSetupById;

public sealed record GetFgsTenantServiceAccountsSetupByIdQuery(long CompanyId)
    : IRequest<ApiResponse<FgsTenantServiceAccountsSetupDetailDto>>;
