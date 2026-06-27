using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadSources.Commands.PatchLeadSource;

public sealed record PatchLeadSourceCommand(long Id, LeadSourcePatchDto Dto)
    : IRequest<ApiResponse<LeadSourceDetailDto>>;
