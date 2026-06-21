using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.UpdateLeadDisqualificationReason;

public sealed record UpdateLeadDisqualificationReasonCommand(long Id, LeadDisqualificationReasonUpdateDto Dto)
    : IRequest<ApiResponse<LeadDisqualificationReasonDetailDto>>;
