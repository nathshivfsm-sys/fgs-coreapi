using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;

namespace Fgs.Setup.Application.Abstractions.CommunicationTemplates;

public interface IFgsSetupCommunicationTemplateWriteService
{
    Task<FgsSetupCommunicationTemplateDetailDto> CreateAsync(FgsSetupCommunicationTemplateCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupCommunicationTemplateDetailDto> UpdateAsync(long id, FgsSetupCommunicationTemplateUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupCommunicationTemplateDetailDto> PatchAsync(long id, FgsSetupCommunicationTemplatePatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsSetupCommunicationTemplateDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
