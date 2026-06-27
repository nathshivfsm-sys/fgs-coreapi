using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.CreateLeadDisqualificationReason;

public sealed record CreateLeadDisqualificationReasonCommand(LeadDisqualificationReasonCreateDto Dto)
    : IRequest<ApiResponse<LeadDisqualificationReasonDetailDto>>;
