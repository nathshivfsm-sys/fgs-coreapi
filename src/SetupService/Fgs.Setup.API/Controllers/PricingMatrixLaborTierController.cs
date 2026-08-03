using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Commands.CreateFgsSetupPricingMatrixLaborTier;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Commands.UpdateFgsSetupPricingMatrixLaborTier;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Commands.PatchFgsSetupPricingMatrixLaborTier;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Queries.GetFgsSetupPricingMatrixLaborTierById;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Queries.ListFgsSetupPricingMatrixLaborTiers;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Queries.LookupFgsSetupPricingMatrixLaborTiers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("pricingmatrixlabortier")]
[Produces("application/json")]
public sealed class PricingMatrixLaborTierController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id,CancellationToken ct){var r=await mediator.Send(new GetFgsSetupPricingMatrixLaborTierByIdQuery(id),ct);return StatusCode(r.StatusCode,r);}
    [HttpGet]
    public async Task<IActionResult> List([FromQuery]int page=1,[FromQuery]int pageSize=25,[FromQuery]string? sortBy=null,[FromQuery]SortDirection sortDirection=SortDirection.Asc,[FromQuery]string? search=null,[FromQuery]bool? isActive=null,
        [FromQuery] long? pricingMatrixLaborId = null,
        CancellationToken ct=default)
    {
        var r=await mediator.Send(new ListFgsSetupPricingMatrixLaborTiersQuery(new SetupListQuery(page,pageSize,sortBy,sortDirection,search,isActive),new FgsSetupPricingMatrixLaborTierListFilters(pricingMatrixLaborId)),ct);return StatusCode(r.StatusCode,r);
    }
    [HttpGet("lookup")]
    public async Task<IActionResult> Lookup([FromQuery]bool activeOnly=true,[FromQuery]long? pricingMatrixLaborId=null,CancellationToken ct=default)
    {var r=await mediator.Send(new LookupFgsSetupPricingMatrixLaborTiersQuery(activeOnly,pricingMatrixLaborId),ct);return StatusCode(r.StatusCode,r);}
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]FgsSetupPricingMatrixLaborTierCreateDto dto,CancellationToken ct){var r=await mediator.Send(new CreateFgsSetupPricingMatrixLaborTierCommand(dto),ct);return StatusCode(r.StatusCode,r);}
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id,[FromBody]FgsSetupPricingMatrixLaborTierUpdateDto dto,CancellationToken ct){var r=await mediator.Send(new UpdateFgsSetupPricingMatrixLaborTierCommand(id,dto),ct);return StatusCode(r.StatusCode,r);}
    [HttpPatch("{id:long}")]
    public async Task<IActionResult> Patch(long id,[FromBody]FgsSetupPricingMatrixLaborTierPatchDto dto,CancellationToken ct){var r=await mediator.Send(new PatchFgsSetupPricingMatrixLaborTierCommand(id,dto),ct);return StatusCode(r.StatusCode,r);}
}
