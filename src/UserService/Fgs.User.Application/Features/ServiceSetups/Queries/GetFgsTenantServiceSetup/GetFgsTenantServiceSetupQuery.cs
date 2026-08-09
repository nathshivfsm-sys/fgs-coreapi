using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceSetups.Queries.GetFgsTenantServiceSetup;

public sealed record GetFgsTenantServiceSetupQuery
    : IRequest<ApiResponse<FgsTenantServiceSetupDetailDto>>;
