using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceAccountsSetups.Queries.GetFgsTenantServiceAccountsSetup;

public sealed record GetFgsTenantServiceAccountsSetupQuery
    : IRequest<ApiResponse<FgsTenantServiceAccountsSetupDetailDto>>;
