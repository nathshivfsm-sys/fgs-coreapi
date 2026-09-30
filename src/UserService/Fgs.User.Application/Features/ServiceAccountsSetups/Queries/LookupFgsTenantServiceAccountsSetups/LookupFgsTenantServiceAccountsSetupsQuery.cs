using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceAccountsSetups.Queries.LookupFgsTenantServiceAccountsSetups;

public sealed record LookupFgsTenantServiceAccountsSetupsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsTenantServiceAccountsSetupLookupDto>>>;
