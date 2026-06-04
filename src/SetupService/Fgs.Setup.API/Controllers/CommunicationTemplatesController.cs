using Asp.Versioning;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Api;
using Fgs.Setup.Application.Common;
using Fgs.Setup.Application.Common.Options;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Internal communication template reads for peer services (Notification, etc.).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("communication-templates")]
[Produces("application/json")]
public sealed class CommunicationTemplatesController(
    FgsSetupDbContext context,
    IOptions<CredentialDistributionOptions> distributionOptions) : ControllerBase
{
    [HttpGet("active")]
    [ProducesResponseType(typeof(CommunicationTemplateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActive(
        [FromQuery] long? tenantId,
        [FromQuery] long? companyId,
        [FromQuery] string templateType,
        [FromQuery] string code,
        [FromHeader(Name = CredentialDistributionHeaders.InternalServiceKey)] string? serviceKey,
        CancellationToken cancellationToken)
    {
        if (!IsInternalServiceAuthorized(serviceKey, distributionOptions.Value))
        {
            return Unauthorized();
        }

        var normalizedType = templateType.Trim();
        var normalizedCode = code.Trim();

        var template = await context.FgsSetupCommunicationTemplates
            .AsNoTracking()
            .Where(t =>
                t.TemplateType == normalizedType
                && t.Code == normalizedCode
                && t.IsActive
                && t.TenantId == tenantId
                && t.CompanyId == companyId)
            .OrderByDescending(t => t.Id)
            .Select(t => new CommunicationTemplateDto(
                t.Id,
                t.TenantId,
                t.CompanyId,
                t.TemplateType,
                t.Code,
                t.Name,
                t.Subject,
                t.Body,
                t.IsMobileVisible,
                t.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        return template is null ? NotFound() : Ok(template);
    }

    private static bool IsInternalServiceAuthorized(
        string? providedKey,
        CredentialDistributionOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.InternalServiceKey))
        {
            return false;
        }

        return string.Equals(providedKey, options.InternalServiceKey, StringComparison.Ordinal);
    }
}
