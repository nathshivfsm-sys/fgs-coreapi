using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Commands.CreateFgsSetupPricingMatrixLabor;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Commands.UpdateFgsSetupPricingMatrixLabor;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Commands.PatchFgsSetupPricingMatrixLabor;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Queries.GetFgsSetupPricingMatrixLaborById;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Queries.ListFgsSetupPricingMatrixLabors;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Queries.LookupFgsSetupPricingMatrixLabors;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("pricingmatrixlabor")]
[Produces("application/json")]
public sealed class PricingMatrixLaborController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id,CancellationToken ct){var r=await mediator.Send(new GetFgsSetupPricingMatrixLaborByIdQuery(id),ct);return StatusCode(r.StatusCode,r);}
    [HttpGet]
    public async Task<IActionResult> List([FromQuery]int page=1,[FromQuery]int pageSize=25,[FromQuery]string? sortBy=null,[FromQuery]SortDirection sortDirection=SortDirection.Asc,[FromQuery]string? search=null,[FromQuery]bool? isActive=null,
        [FromQuery] long? pricingMatrixId = null,
        CancellationToken ct=default)
    {
        var r=await mediator.Send(new ListFgsSetupPricingMatrixLaborsQuery(new SetupListQuery(page,pageSize,sortBy,sortDirection,search,isActive),new FgsSetupPricingMatrixLaborListFilters(pricingMatrixId)),ct);return StatusCode(r.StatusCode,r);
    }
    [HttpGet("lookup")]
    public async Task<IActionResult> Lookup([FromQuery]bool activeOnly=true,[FromQuery]long? pricingMatrixId=null,CancellationToken ct=default)
    {var r=await mediator.Send(new LookupFgsSetupPricingMatrixLaborsQuery(activeOnly,pricingMatrixId),ct);return StatusCode(r.StatusCode,r);}
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]FgsSetupPricingMatrixLaborCreateDto dto,CancellationToken ct){var r=await mediator.Send(new CreateFgsSetupPricingMatrixLaborCommand(dto),ct);return StatusCode(r.StatusCode,r);}
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id,[FromBody]FgsSetupPricingMatrixLaborUpdateDto dto,CancellationToken ct){var r=await mediator.Send(new UpdateFgsSetupPricingMatrixLaborCommand(id,dto),ct);return StatusCode(r.StatusCode,r);}
    [HttpPatch("{id:long}")]
    public async Task<IActionResult> Patch(long id,[FromBody]FgsSetupPricingMatrixLaborPatchDto dto,CancellationToken ct){var r=await mediator.Send(new PatchFgsSetupPricingMatrixLaborCommand(id,dto),ct);return StatusCode(r.StatusCode,r);}
}
