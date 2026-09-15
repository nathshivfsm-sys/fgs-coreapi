using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloAccountingIntegrationTypes;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloAppointmentAssignmentEventTypes;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloBusinessTypes;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloCountries;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloInventoryTransactionSourceTypes;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloInventoryTransactionTypes;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloLanguages;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloLocationTypes;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloMasterEntityTypes;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloSetupDescriptionTypes;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloSetupTenantStatuses;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloStateProvinces;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloTimeZones;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloUnitOfMeasures;
using Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloVehicleMaintenanceTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Global (non-tenant) catalog lookup endpoints under /api/v1/glo.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("glo")]
public sealed class GloLookupController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [AllowAnonymous]
    [HttpGet("country/lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GloCountryLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LookupCountries(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupGloCountriesQuery(activeOnly), cancellationToken));

    [HttpGet("stateprovince/lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GloStateProvinceLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LookupStateProvinces(
        [FromQuery] string countryCode,
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupGloStateProvincesQuery(countryCode, activeOnly), cancellationToken));

    [AllowAnonymous]
    [HttpGet("timezone/lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GloTimeZoneLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LookupTimeZones(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupGloTimeZonesQuery(activeOnly), cancellationToken));

    [HttpGet("language/lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GloLanguageLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LookupLanguages(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupGloLanguagesQuery(activeOnly), cancellationToken));

    [HttpGet("locationtype/lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GloLocationTypeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LookupLocationTypes(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupGloLocationTypesQuery(activeOnly), cancellationToken));

    [AllowAnonymous]
    [HttpGet("businesstype/lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GloBusinessTypeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LookupBusinessTypes(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupGloBusinessTypesQuery(activeOnly), cancellationToken));

    [HttpGet("setupdescriptiontype/lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GloSetupDescriptionTypeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LookupSetupDescriptionTypes(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupGloSetupDescriptionTypesQuery(activeOnly), cancellationToken));

    [HttpGet("masterentitytype/lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GloMasterEntityTypeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LookupMasterEntityTypes(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupGloMasterEntityTypesQuery(activeOnly), cancellationToken));

    [HttpGet("unitofmeasure/lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GloUnitOfMeasureLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LookupUnitOfMeasures(
        [FromQuery] string? unitType = null,
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupGloUnitOfMeasuresQuery(unitType, activeOnly), cancellationToken));

    [HttpGet("inventorytransactionsource/lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GloInventoryTransactionSourceTypeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LookupInventoryTransactionSources(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupGloInventoryTransactionSourceTypesQuery(activeOnly), cancellationToken));

    [HttpGet("inventorytransactiontype/lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GloInventoryTransactionTypeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LookupInventoryTransactionTypes(
        [FromQuery] int? sourceTypeId = null,
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupGloInventoryTransactionTypesQuery(sourceTypeId, activeOnly), cancellationToken));

    [HttpGet("accountingintegrationtype/lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GloAccountingIntegrationTypeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LookupAccountingIntegrationTypes(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupGloAccountingIntegrationTypesQuery(activeOnly), cancellationToken));

    [HttpGet("appointmentassignmenteventtype/lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GloAppointmentAssignmentEventTypeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LookupAppointmentAssignmentEventTypes(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupGloAppointmentAssignmentEventTypesQuery(activeOnly), cancellationToken));

    [HttpGet("vehiclemaintenancetype/lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GloVehicleMaintenanceTypeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LookupVehicleMaintenanceTypes(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupGloVehicleMaintenanceTypesQuery(activeOnly), cancellationToken));

    [HttpGet("setuptenantstatus/lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GloSetupTenantStatusLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LookupSetupTenantStatuses(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupGloSetupTenantStatusesQuery(activeOnly), cancellationToken));
}
