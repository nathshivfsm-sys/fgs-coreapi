using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Queries.GetFgsSetupCommunicationTemplateById;

public sealed record GetFgsSetupCommunicationTemplateByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSetupCommunicationTemplateDetailDto>>;
