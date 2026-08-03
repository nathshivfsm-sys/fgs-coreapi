using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceAccountsSetups.Commands.UpdateFgsTenantServiceAccountsSetup;

public sealed record UpdateFgsTenantServiceAccountsSetupCommand(FgsTenantServiceAccountsSetupUpdateDto Dto)
    : IRequest<ApiResponse<FgsTenantServiceAccountsSetupDetailDto>>;
