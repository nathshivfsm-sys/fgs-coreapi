using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadSources.Commands.DeleteLeadSource;

public sealed record DeleteLeadSourceCommand(long Id)
    : IRequest<ApiResponse<LeadSourceDetailDto>>;
