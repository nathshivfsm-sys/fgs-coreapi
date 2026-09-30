using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceSetups.Queries.LookupFgsTenantServiceSetups;

public sealed record LookupFgsTenantServiceSetupsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsTenantServiceSetupLookupDto>>>;
