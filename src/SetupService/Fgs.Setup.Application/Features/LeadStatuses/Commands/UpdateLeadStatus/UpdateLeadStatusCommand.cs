using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadStatuses.Commands.UpdateLeadStatus;

public sealed record UpdateLeadStatusCommand(long Id, LeadStatusUpdateDto Dto)
    : IRequest<ApiResponse<LeadStatusDetailDto>>;
