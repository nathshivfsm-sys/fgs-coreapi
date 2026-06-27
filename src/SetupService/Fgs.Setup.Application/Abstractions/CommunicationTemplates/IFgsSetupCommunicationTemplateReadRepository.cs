using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;

namespace Fgs.Setup.Application.Abstractions.CommunicationTemplates;

public interface IFgsSetupCommunicationTemplateReadRepository
{
    Task<FgsSetupCommunicationTemplateDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSetupCommunicationTemplateSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupCommunicationTemplateListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSetupCommunicationTemplateLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCommunicationChannelAndTemplateTypeAndCodeAsync(
        string communicationChannel, string templateType, string code,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
