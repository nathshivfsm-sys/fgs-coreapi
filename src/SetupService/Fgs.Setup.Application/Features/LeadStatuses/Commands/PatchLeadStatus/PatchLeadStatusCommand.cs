using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadStatuses.Commands.PatchLeadStatus;

public sealed record PatchLeadStatusCommand(long Id, LeadStatusPatchDto Dto)
    : IRequest<ApiResponse<LeadStatusDetailDto>>;
