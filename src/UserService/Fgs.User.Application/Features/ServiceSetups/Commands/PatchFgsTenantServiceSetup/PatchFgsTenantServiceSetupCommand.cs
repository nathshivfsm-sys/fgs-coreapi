using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceSetups.Commands.PatchFgsTenantServiceSetup;

public sealed record PatchFgsTenantServiceSetupCommand(FgsTenantServiceSetupPatchDto Dto)
    : IRequest<ApiResponse<FgsTenantServiceSetupDetailDto>>;
