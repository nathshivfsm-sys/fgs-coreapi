using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Credentials;
using Fgs.Credentials.Options;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.CommunicationTemplates.Commands.CreateFgsSetupCommunicationTemplate;
using Fgs.Setup.Application.Features.CommunicationTemplates.Commands.DeleteFgsSetupCommunicationTemplate;
using Fgs.Setup.Application.Features.CommunicationTemplates.Commands.PatchFgsSetupCommunicationTemplate;
using Fgs.Setup.Application.Features.CommunicationTemplates.Commands.UpdateFgsSetupCommunicationTemplate;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using Fgs.Setup.Application.Features.CommunicationTemplates.Queries.GetActiveCommunicationTemplate;
using Fgs.Setup.Application.Features.CommunicationTemplates.Queries.GetFgsSetupCommunicationTemplateById;
using Fgs.Setup.Application.Features.CommunicationTemplates.Queries.ListCommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates.Queries.LookupCommunicationTemplates;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Global and tenant-scoped communication template catalog management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("communication-templates")]
[Produces("application/json")]
public sealed class CommunicationTemplatesController(
    IMediator mediator,
    IOptions<CredentialDistributionOptions> distributionOptions) : FgsApiControllerBase(mediator)
{
    /// <summary>
    /// Resolves the active template for a tenant/company scope.
    /// Accepts JWT or <see cref="CredentialDistributionHeaders.InternalServiceKey"/> for service-to-service calls.
    /// </summary>
    [AllowAnonymous]
    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<CommunicationTemplateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActive(
        [FromQuery] long? tenantId,
        [FromQuery] long? companyId,
        [FromQuery] string templateType,
        [FromQuery] string code,
        [FromHeader(Name = CredentialDistributionHeaders.InternalServiceKey)] string? serviceKey,
        CancellationToken cancellationToken)
    {
        var isInternalService = InternalServiceAuthorization.IsAuthorized(
            serviceKey,
            distributionOptions.Value);
        if (!isInternalService && User?.Identity?.IsAuthenticated != true)
        {
            return StatusCode(
                StatusCodes.Status401Unauthorized,
                ApiResponse<object>.Fail(["Unauthorized."], ApiStatusCodes.Unauthorized));
        }

        return FromApiResponse(await Mediator.Send(
            new GetActiveCommunicationTemplateQuery(
                tenantId,
                companyId,
                templateType,
                code),
            cancellationToken));
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupCommunicationTemplateDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetFgsSetupCommunicationTemplateByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsSetupCommunicationTemplateSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? communicationChannel = null,
        [FromQuery] string? templateType = null,
        [FromQuery] string? code = null,
        [FromQuery] string? name = null,
        CancellationToken cancellationToken = default)
    {
        var response = await Mediator.Send(
            new ListCommunicationTemplatesQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsSetupCommunicationTemplateListFilters(communicationChannel, templateType, code, name)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsSetupCommunicationTemplateLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await Mediator.Send(new LookupCommunicationTemplatesQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupCommunicationTemplateDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsSetupCommunicationTemplateCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new CreateFgsSetupCommunicationTemplateCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupCommunicationTemplateDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsSetupCommunicationTemplateUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new UpdateFgsSetupCommunicationTemplateCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupCommunicationTemplateDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsSetupCommunicationTemplatePatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new PatchFgsSetupCommunicationTemplateCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupCommunicationTemplateDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new DeleteFgsSetupCommunicationTemplateCommand(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
