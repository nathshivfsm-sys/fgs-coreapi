using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Queries.GetActiveCommunicationTemplate;

public sealed record GetActiveCommunicationTemplateQuery(
    long? TenantId,
    long? CompanyId,
    string TemplateType,
    string Code,
    string? InternalServiceKey) : IRequest<ApiResponse<CommunicationTemplateDto>>;
