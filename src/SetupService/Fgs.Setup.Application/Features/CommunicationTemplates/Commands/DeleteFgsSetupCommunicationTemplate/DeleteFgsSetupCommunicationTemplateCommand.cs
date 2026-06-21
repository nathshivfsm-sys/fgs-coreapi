using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Commands.DeleteFgsSetupCommunicationTemplate;

public sealed record DeleteFgsSetupCommunicationTemplateCommand(long Id)
    : IRequest<ApiResponse<FgsSetupCommunicationTemplateDetailDto>>;
