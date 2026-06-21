using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Queries.LookupCommunicationTemplates;

public sealed class LookupCommunicationTemplatesQueryHandler(IFgsSetupCommunicationTemplateReadRepository readRepository)
    : IRequestHandler<LookupCommunicationTemplatesQuery, ApiResponse<IReadOnlyList<FgsSetupCommunicationTemplateLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupCommunicationTemplateLookupDto>>> Handle(
        LookupCommunicationTemplatesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSetupCommunicationTemplateLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSetupCommunicationTemplateLookupDto>>(ex);
        }
    }
}
