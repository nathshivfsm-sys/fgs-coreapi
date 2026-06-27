using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadSources.Commands.CreateLeadSource;

public sealed record CreateLeadSourceCommand(LeadSourceCreateDto Dto)
    : IRequest<ApiResponse<LeadSourceDetailDto>>;
