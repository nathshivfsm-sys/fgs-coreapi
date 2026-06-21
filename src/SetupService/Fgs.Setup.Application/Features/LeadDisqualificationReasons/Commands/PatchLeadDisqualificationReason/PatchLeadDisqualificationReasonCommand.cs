using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.PatchLeadDisqualificationReason;

public sealed record PatchLeadDisqualificationReasonCommand(long Id, LeadDisqualificationReasonPatchDto Dto)
    : IRequest<ApiResponse<LeadDisqualificationReasonDetailDto>>;
