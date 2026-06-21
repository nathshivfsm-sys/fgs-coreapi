using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Queries.LookupCommunicationTemplates;

public sealed record LookupCommunicationTemplatesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsSetupCommunicationTemplateLookupDto>>>;
