using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceSetups.Commands.UpdateFgsTenantServiceSetup;

public sealed record UpdateFgsTenantServiceSetupCommand(FgsTenantServiceSetupUpdateDto Dto)
    : IRequest<ApiResponse<FgsTenantServiceSetupDetailDto>>;
