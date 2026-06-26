using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Queries.ListActiveCommunicationTemplates;

public sealed record ListActiveCommunicationTemplatesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsSetupCommunicationTemplateListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsSetupCommunicationTemplateSummaryDto>>>;
