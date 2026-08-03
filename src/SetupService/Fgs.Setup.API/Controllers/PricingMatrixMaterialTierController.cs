using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Commands.CreateFgsSetupPricingMatrixMaterialTier;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Commands.UpdateFgsSetupPricingMatrixMaterialTier;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Commands.PatchFgsSetupPricingMatrixMaterialTier;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Queries.GetFgsSetupPricingMatrixMaterialTierById;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Queries.ListFgsSetupPricingMatrixMaterialTiers;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Queries.LookupFgsSetupPricingMatrixMaterialTiers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("pricingmatrixmaterialtier")]
[Produces("application/json")]
public sealed class PricingMatrixMaterialTierController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id,CancellationToken ct){var r=await mediator.Send(new GetFgsSetupPricingMatrixMaterialTierByIdQuery(id),ct);return StatusCode(r.StatusCode,r);}
    [HttpGet]
    public async Task<IActionResult> List([FromQuery]int page=1,[FromQuery]int pageSize=25,[FromQuery]string? sortBy=null,[FromQuery]SortDirection sortDirection=SortDirection.Asc,[FromQuery]string? search=null,[FromQuery]bool? isActive=null,
        [FromQuery] long? pricingMatrixId = null,
        CancellationToken ct=default)
    {
        var r=await mediator.Send(new ListFgsSetupPricingMatrixMaterialTiersQuery(new SetupListQuery(page,pageSize,sortBy,sortDirection,search,isActive),new FgsSetupPricingMatrixMaterialTierListFilters(pricingMatrixId)),ct);return StatusCode(r.StatusCode,r);
    }
    [HttpGet("lookup")]
    public async Task<IActionResult> Lookup([FromQuery]bool activeOnly=true,[FromQuery]long? pricingMatrixId=null,CancellationToken ct=default)
    {var r=await mediator.Send(new LookupFgsSetupPricingMatrixMaterialTiersQuery(activeOnly,pricingMatrixId),ct);return StatusCode(r.StatusCode,r);}
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]FgsSetupPricingMatrixMaterialTierCreateDto dto,CancellationToken ct){var r=await mediator.Send(new CreateFgsSetupPricingMatrixMaterialTierCommand(dto),ct);return StatusCode(r.StatusCode,r);}
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id,[FromBody]FgsSetupPricingMatrixMaterialTierUpdateDto dto,CancellationToken ct){var r=await mediator.Send(new UpdateFgsSetupPricingMatrixMaterialTierCommand(id,dto),ct);return StatusCode(r.StatusCode,r);}
    [HttpPatch("{id:long}")]
    public async Task<IActionResult> Patch(long id,[FromBody]FgsSetupPricingMatrixMaterialTierPatchDto dto,CancellationToken ct){var r=await mediator.Send(new PatchFgsSetupPricingMatrixMaterialTierCommand(id,dto),ct);return StatusCode(r.StatusCode,r);}
}
