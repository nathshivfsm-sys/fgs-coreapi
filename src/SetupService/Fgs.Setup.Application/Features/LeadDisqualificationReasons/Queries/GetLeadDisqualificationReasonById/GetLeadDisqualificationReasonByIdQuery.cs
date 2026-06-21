using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.GetLeadDisqualificationReasonById;

public sealed record GetLeadDisqualificationReasonByIdQuery(long Id)
    : IRequest<ApiResponse<LeadDisqualificationReasonDetailDto>>;
