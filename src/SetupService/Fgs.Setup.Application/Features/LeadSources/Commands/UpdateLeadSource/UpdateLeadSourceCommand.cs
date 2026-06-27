using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadSources.Commands.UpdateLeadSource;

public sealed record UpdateLeadSourceCommand(long Id, LeadSourceUpdateDto Dto)
    : IRequest<ApiResponse<LeadSourceDetailDto>>;
