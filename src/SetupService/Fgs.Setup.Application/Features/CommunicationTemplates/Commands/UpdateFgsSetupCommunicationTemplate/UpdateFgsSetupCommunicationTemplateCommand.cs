using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Commands.UpdateFgsSetupCommunicationTemplate;

public sealed record UpdateFgsSetupCommunicationTemplateCommand(long Id, FgsSetupCommunicationTemplateUpdateDto Dto)
    : IRequest<ApiResponse<FgsSetupCommunicationTemplateDetailDto>>;
