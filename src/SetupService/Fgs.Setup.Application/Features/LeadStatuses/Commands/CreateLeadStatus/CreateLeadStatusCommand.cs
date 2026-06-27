using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadStatuses.Commands.CreateLeadStatus;

public sealed record CreateLeadStatusCommand(LeadStatusCreateDto Dto)
    : IRequest<ApiResponse<LeadStatusDetailDto>>;
