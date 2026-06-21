using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Queries.ListCommunicationTemplates;

public sealed record ListCommunicationTemplatesQuery(
    SetupListQuery Query, FgsSetupCommunicationTemplateListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSetupCommunicationTemplateSummaryDto>>>;
