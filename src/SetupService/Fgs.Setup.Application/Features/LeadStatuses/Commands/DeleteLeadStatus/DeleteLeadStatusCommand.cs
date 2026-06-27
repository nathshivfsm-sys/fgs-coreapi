using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadStatuses.Commands.DeleteLeadStatus;

public sealed record DeleteLeadStatusCommand(long Id)
    : IRequest<ApiResponse<LeadStatusDetailDto>>;
