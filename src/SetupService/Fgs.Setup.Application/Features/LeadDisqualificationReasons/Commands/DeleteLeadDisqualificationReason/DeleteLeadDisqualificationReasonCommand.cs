using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.DeleteLeadDisqualificationReason;

public sealed record DeleteLeadDisqualificationReasonCommand(long Id)
    : IRequest<ApiResponse<LeadDisqualificationReasonDetailDto>>;
