using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Commands.CreateFgsSetupPricingMatrixOther;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Commands.UpdateFgsSetupPricingMatrixOther;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Commands.PatchFgsSetupPricingMatrixOther;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Queries.GetFgsSetupPricingMatrixOtherById;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Queries.ListFgsSetupPricingMatrixOthers;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Queries.LookupFgsSetupPricingMatrixOthers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("pricingmatrixother")]
[Produces("application/json")]
public sealed class PricingMatrixOtherController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id,CancellationToken ct){var r=await mediator.Send(new GetFgsSetupPricingMatrixOtherByIdQuery(id),ct);return StatusCode(r.StatusCode,r);}
    [HttpGet]
    public async Task<IActionResult> List([FromQuery]int page=1,[FromQuery]int pageSize=25,[FromQuery]string? sortBy=null,[FromQuery]SortDirection sortDirection=SortDirection.Asc,[FromQuery]string? search=null,[FromQuery]bool? isActive=null,
        [FromQuery] long? pricingMatrixId = null,
        [FromQuery] string? categoryCode = null,
        CancellationToken ct=default)
    {
        var r=await mediator.Send(new ListFgsSetupPricingMatrixOthersQuery(new SetupListQuery(page,pageSize,sortBy,sortDirection,search,isActive),new FgsSetupPricingMatrixOtherListFilters(pricingMatrixId, categoryCode)),ct);return StatusCode(r.StatusCode,r);
    }
    [HttpGet("lookup")]
    public async Task<IActionResult> Lookup([FromQuery]bool activeOnly=true,[FromQuery]long? pricingMatrixId=null,CancellationToken ct=default)
    {var r=await mediator.Send(new LookupFgsSetupPricingMatrixOthersQuery(activeOnly,pricingMatrixId),ct);return StatusCode(r.StatusCode,r);}
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]FgsSetupPricingMatrixOtherCreateDto dto,CancellationToken ct){var r=await mediator.Send(new CreateFgsSetupPricingMatrixOtherCommand(dto),ct);return StatusCode(r.StatusCode,r);}
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id,[FromBody]FgsSetupPricingMatrixOtherUpdateDto dto,CancellationToken ct){var r=await mediator.Send(new UpdateFgsSetupPricingMatrixOtherCommand(id,dto),ct);return StatusCode(r.StatusCode,r);}
    [HttpPatch("{id:long}")]
    public async Task<IActionResult> Patch(long id,[FromBody]FgsSetupPricingMatrixOtherPatchDto dto,CancellationToken ct){var r=await mediator.Send(new PatchFgsSetupPricingMatrixOtherCommand(id,dto),ct);return StatusCode(r.StatusCode,r);}
}
