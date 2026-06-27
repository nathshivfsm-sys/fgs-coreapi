using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Commands.PatchFgsSetupCommunicationTemplate;

public sealed record PatchFgsSetupCommunicationTemplateCommand(long Id, FgsSetupCommunicationTemplatePatchDto Dto)
    : IRequest<ApiResponse<FgsSetupCommunicationTemplateDetailDto>>;
