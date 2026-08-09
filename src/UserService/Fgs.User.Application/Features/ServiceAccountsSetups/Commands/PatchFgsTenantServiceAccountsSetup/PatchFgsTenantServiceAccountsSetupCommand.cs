using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceAccountsSetups.Commands.PatchFgsTenantServiceAccountsSetup;

public sealed record PatchFgsTenantServiceAccountsSetupCommand(FgsTenantServiceAccountsSetupPatchDto Dto)
    : IRequest<ApiResponse<FgsTenantServiceAccountsSetupDetailDto>>;
