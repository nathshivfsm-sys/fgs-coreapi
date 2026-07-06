#!/usr/bin/env python3
"""Asset Service catalog CRUD generator (invoked by Generate-AssetCrud-Full.ps1)."""
from __future__ import annotations

import os
import textwrap

ROOT = os.path.normpath(os.path.join(os.path.dirname(__file__), ".."))
GATEWAY = os.path.normpath(os.path.join(ROOT, "..", "Gateway"))
CREATED: list[str] = []

SKIP = {
    "Fgs.Asset.Application/Common/AssetCrud/AssetListQuery.cs",
    "Fgs.Asset.Application/Abstractions/Persistence/IAssetReadConnectionFactory.cs",
    "Fgs.Asset.Application/Abstractions/Time/IDateTimeProvider.cs",
    "Fgs.Asset.Infrastructure/Database/FgsAssetConnectionString.cs",
    "Fgs.Asset.Infrastructure/Database/Read/FgsAssetReadConnectionFactory.cs",
    "Fgs.Asset.Infrastructure/Common/AssetTenantScopeResolver.cs",
    "Fgs.Asset.Infrastructure/Common/Time/DateTimeProvider.cs",
    "Fgs.Asset.Infrastructure/Common/AssetEntityAuditHelper.cs",
}


def write(path: str, content: str) -> None:
    norm = path.replace("\\", "/")
    if norm in SKIP:
        return
    full = os.path.join(ROOT, path.replace("/", os.sep))
    os.makedirs(os.path.dirname(full), exist_ok=True)
    with open(full, "w", encoding="utf-8", newline="\n") as f:
        f.write(content.rstrip() + "\n")
    CREATED.append(norm)


def gen_commands_queries_controller(
    e: str,
    f: str,
    r: str,
    display_cap: str,
    controller: str,
    list_filter_params: str,
    list_filter_ctor: str,
    has_post: bool = True,
    has_is_active: bool = True,
    lookup_active: bool = True,
) -> None:
    cmds = ["Create", "Update", "Patch"] if has_post else ["Update", "Patch"]
    for cmd in cmds:
        sig = f"{e}CreateDto Dto" if cmd == "Create" else f"long Id, {e}{cmd}Dto Dto"
        write(
            f"Fgs.Asset.Application/Features/{f}/Commands/{cmd}{e}/{cmd}{e}Command.cs",
            f"""using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.{f}.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.{f}.Commands.{cmd}{e};

public sealed record {cmd}{e}Command({sig})
    : IRequest<ApiResponse<{e}DetailDto>>;
""",
        )
        call = "request.Dto" if cmd == "Create" else "request.Id, request.Dto"
        ret = (
            f"return ApiResponse<{e}DetailDto>.Ok(result, ApiStatusCodes.Created);"
            if cmd == "Create"
            else f"return ApiResponse<{e}DetailDto>.Ok(result);"
        )
        write(
            f"Fgs.Asset.Application/Features/{f}/Commands/{cmd}{e}/{cmd}{e}CommandHandler.cs",
            f"""using Fgs.Asset.Application.Abstractions.{f};
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Asset.Application.Features.{f}.Commands.{cmd}{e};

public sealed class {cmd}{e}CommandHandler(
    I{e}WriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<{cmd}{e}CommandHandler> logger)
    : IRequestHandler<{cmd}{e}Command, ApiResponse<{e}DetailDto>>
{{
    public async Task<ApiResponse<{e}DetailDto>> Handle(
        {cmd}{e}Command request,
        CancellationToken cancellationToken)
    {{
        var result = await writeService.{cmd}Async({call}, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "{r}"),
            cancellationToken);
        {ret}
    }}
}}
""",
        )

    write(
        f"Fgs.Asset.Application/Features/{f}/Queries/Get{e}ById/Get{e}ByIdQuery.cs",
        f"""using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.{f}.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.{f}.Queries.Get{e}ById;

public sealed record Get{e}ByIdQuery(long Id) : IRequest<ApiResponse<{e}DetailDto>>;
""",
    )
    write(
        f"Fgs.Asset.Application/Features/{f}/Queries/Get{e}ById/Get{e}ByIdQueryHandler.cs",
        f"""using Fgs.Asset.Application.Abstractions.{f};
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.{f}.Queries.Get{e}ById;

public sealed class Get{e}ByIdQueryHandler(
    I{e}ReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<Get{e}ByIdQuery, ApiResponse<{e}DetailDto>>
{{
    public async Task<ApiResponse<{e}DetailDto>> Handle(
        Get{e}ByIdQuery request,
        CancellationToken cancellationToken)
    {{
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "{r}",
            request.Id.ToString());

        var cached = await cache.GetAsync<{e}DetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {{
            return ApiResponse<{e}DetailDto>.Ok(cached);
        }}

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {{
            return ApiResponse<{e}DetailDto>.Fail(
                [$"{display_cap} '{{request.Id}}' was not found."],
                ApiStatusCodes.NotFound);
        }}

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<{e}DetailDto>.Ok(result);
    }}
}}
""",
    )

    list_q = f"List{f}"
    write(
        f"Fgs.Asset.Application/Features/{f}/Queries/{list_q}/{list_q}Query.cs",
        f"""using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.{f}.Queries.{list_q};

public sealed record {list_q}Query(AssetListQuery Query, {e}ListFilters Filters)
    : IRequest<ApiResponse<PagedResult<{e}SummaryDto>>>;
""",
    )
    write(
        f"Fgs.Asset.Application/Features/{f}/Queries/{list_q}/{list_q}QueryHandler.cs",
        f"""using Fgs.Asset.Application.Abstractions.{f};
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.{f}.Queries.{list_q};

public sealed class {list_q}QueryHandler(I{e}ReadRepository readRepository)
    : IRequestHandler<{list_q}Query, ApiResponse<PagedResult<{e}SummaryDto>>>
{{
    public async Task<ApiResponse<PagedResult<{e}SummaryDto>>> Handle(
        {list_q}Query request,
        CancellationToken cancellationToken)
    {{
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<{e}SummaryDto>>.Ok(result);
    }}
}}
""",
    )

    lookup_q = f"Lookup{f}"
    lookup_sig = "bool ActiveOnly = true" if lookup_active else ""
    lookup_record = f"Lookup{f}Query({lookup_sig})" if lookup_sig else f"Lookup{f}Query()"
    write(
        f"Fgs.Asset.Application/Features/{f}/Queries/{lookup_q}/{lookup_q}Query.cs",
        f"""using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.{f}.Queries.{lookup_q};

public sealed record {lookup_q}Query({lookup_sig})
    : IRequest<ApiResponse<IReadOnlyList<{e}LookupDto>>>;
""",
    )
    lookup_call = "request.ActiveOnly, " if lookup_active else ""
    write(
        f"Fgs.Asset.Application/Features/{f}/Queries/{lookup_q}/{lookup_q}QueryHandler.cs",
        f"""using Fgs.Asset.Application.Abstractions.{f};
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.{f}.Queries.{lookup_q};

public sealed class {lookup_q}QueryHandler(I{e}ReadRepository readRepository)
    : IRequestHandler<{lookup_q}Query, ApiResponse<IReadOnlyList<{e}LookupDto>>>
{{
    public async Task<ApiResponse<IReadOnlyList<{e}LookupDto>>> Handle(
        {lookup_q}Query request,
        CancellationToken cancellationToken)
    {{
        var result = await readRepository.LookupAsync({lookup_call}cancellationToken);
        return ApiResponse<IReadOnlyList<{e}LookupDto>>.Ok(result);
    }}
}}
""",
    )

    create_using = (
        f"using Fgs.Asset.Application.Features.{f}.Commands.Create{e};\n" if has_post else ""
    )
    is_active_line = "        [FromQuery] bool? isActive = true,\n" if has_is_active else ""
    is_active_arg = "isActive" if has_is_active else "null"
    lookup_param = "        [FromQuery] bool activeOnly = true,\n" if lookup_active else ""
    lookup_arg = "activeOnly" if lookup_active else ""
    post_block = ""
    if has_post:
        post_block = f"""
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<{e}DetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] {e}CreateDto request,
        CancellationToken cancellationToken)
    {{
        var response = await mediator.Send(new Create{e}Command(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }}
"""

    write(
        f"Fgs.Asset.API/Controllers/{controller}.cs",
        f"""using Asp.Versioning;
using Fgs.Asset.Application.Common.AssetCrud;
{create_using}using Fgs.Asset.Application.Features.{f}.Commands.Patch{e};
using Fgs.Asset.Application.Features.{f}.Commands.Update{e};
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Asset.Application.Features.{f}.Queries.Get{e}ById;
using Fgs.Asset.Application.Features.{f}.Queries.{list_q};
using Fgs.Asset.Application.Features.{f}.Queries.{lookup_q};
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Asset.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("{r}")]
[Produces("application/json")]
public sealed class {controller}(IMediator mediator) : ControllerBase
{{
    [HttpGet("{{id:long}}")]
    [ProducesResponseType(typeof(ApiResponse<{e}DetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {{
        var response = await mediator.Send(new Get{e}ByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }}

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<{e}SummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
{is_active_line}{list_filter_params}
        CancellationToken cancellationToken = default)
    {{
        var response = await mediator.Send(
            new {list_q}Query(
                new AssetListQuery(page, pageSize, sortBy, sortDirection, search, {is_active_arg}),
                {list_filter_ctor}),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }}

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<{e}LookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
{lookup_param}        CancellationToken cancellationToken = default)
    {{
        var response = await mediator.Send(new {lookup_q}Query({lookup_arg}), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }}
{post_block}
    [HttpPut("{{id:long}}")]
    [ProducesResponseType(typeof(ApiResponse<{e}DetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] {e}UpdateDto request,
        CancellationToken cancellationToken)
    {{
        var response = await mediator.Send(new Update{e}Command(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }}

    [HttpPatch("{{id:long}}")]
    [ProducesResponseType(typeof(ApiResponse<{e}DetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] {e}PatchDto request,
        CancellationToken cancellationToken)
    {{
        var response = await mediator.Send(new Patch{e}Command(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }}
}}
""",
    )


def gen_simple_tests(e: str, f: str, r: str, display: str, display_cap: str) -> None:
    write(
        f"Fgs.Asset.Tests/{f}/{e}ValidatorTests.cs",
        f"""using Fgs.Asset.Application.Abstractions.{f};
using Fgs.Asset.Application.Features.{f}.Commands.Create{e};
using Fgs.Asset.Application.Features.{f}.Commands.Update{e};
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Asset.Application.Features.{f}.Validators;
using Moq;

namespace Fgs.Asset.Tests.{f};

public sealed class {e}ValidatorTests
{{
    private readonly Mock<I{e}ReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenCodeMissing_HasValidationError()
    {{
        var validator = new Create{e}CommandValidator(_readRepository.Object);
        var command = new Create{e}Command(new {e}CreateDto("", "Name", null));
        var result = await validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(err => err.PropertyName == "Dto.Code");
    }}

    [Fact]
    public async Task CreateValidator_WhenCodeNotUppercase_HasValidationError()
    {{
        var validator = new Create{e}CommandValidator(_readRepository.Object);
        var command = new Create{e}Command(new {e}CreateDto("test", "Name", null));
        var result = await validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(err => err.PropertyName == "Dto.Code");
    }}

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {{
        _readRepository.Setup(repo => repo.ExistsByCodeAsync("TEST", 5, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var validator = new Update{e}CommandValidator(_readRepository.Object);
        var command = new Update{e}Command(5, new {e}UpdateDto("TEST", "Name", null));
        var result = await validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }}
}}
""",
    )
    write(
        f"Fgs.Asset.Tests/{f}/{e}CommandHandlerTests.cs",
        f"""using Fgs.Asset.Application.Features.{f}.Commands.Create{e};
using Fgs.Asset.Application.Features.{f}.Commands.Patch{e};
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Common.Time;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Asset.Infrastructure.{f};
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Asset.Tests.{f};

public sealed class {e}CommandHandlerTests
{{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesActiveRecord()
    {{
        await using var context = await CreateContextAsync();
        var handler = new Create{e}CommandHandler(
            CreateWriteService(context),
            new Mock<ICacheService>().Object,
            CreateTenantAccessor(),
            NullLogger<Create{e}CommandHandler>.Instance);
        var response = await handler.Handle(
            new Create{e}Command(new {e}CreateDto("CODE01", "Test {display_cap}", null)),
            CancellationToken.None);
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
    }}

    [Fact]
    public async Task PatchHandler_SoftDeletesViaIsActive()
    {{
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantAccessor();
        var created = await new Create{e}CommandHandler(
            writeService, cache.Object, tenantAccessor, NullLogger<Create{e}CommandHandler>.Instance)
            .Handle(new Create{e}Command(new {e}CreateDto("CODE01", "Test", null)), CancellationToken.None);
        var response = await new Patch{e}CommandHandler(
            writeService, cache.Object, tenantAccessor, NullLogger<Patch{e}CommandHandler>.Instance)
            .Handle(new Patch{e}Command(created.Data!.Id, new {e}PatchDto(null, null, null, false)), CancellationToken.None);
        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }}

    private static ITenantContextAccessor CreateTenantAccessor() =>
        new TestTenantContextAccessor {{ Current = new TenantContext {{ TenantId = TenantId, CompanyId = CompanyId }} }};

    private static {e}WriteService CreateWriteService(FgsAssetDbContext context)
    {{
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        var tenantAccessor = CreateTenantAccessor();
        var auditHelper = new AssetEntityAuditHelper(userContext.Object, tenantAccessor, new DateTimeProvider());
        return new {e}WriteService(context, new EfUnitOfWork<FgsAssetDbContext>(context), auditHelper);
    }}

    private static async Task<FgsAssetDbContext> CreateContextAsync()
    {{
        var options = new DbContextOptionsBuilder<FgsAssetDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new FgsAssetDbContext(options);
        await context.Database.EnsureCreatedAsync();
        return context;
    }}

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {{
        public ITenantContext? Current {{ get; set; }}
    }}
}}
""",
    )
    write(
        f"Fgs.Asset.Tests/{f}/{e}QueryHandlerTests.cs",
        f"""using Fgs.Asset.Application.Abstractions.{f};
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Asset.Application.Features.{f}.Queries.Get{e}ById;
using Fgs.Asset.Application.Features.{f}.Queries.List{f};
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Moq;

namespace Fgs.Asset.Tests.{f};

public sealed class {e}QueryHandlerTests
{{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {{
        var readRepository = new Mock<I{e}ReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(
            new {e}DetailDto(1, "CODE01", "Test", null, true));
        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext {{ TenantId = 10, CompanyId = 20 }});
        var handler = new Get{e}ByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new Get{e}ByIdQuery(1), CancellationToken.None);
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }}

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {{
        var readRepository = new Mock<I{e}ReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync(({e}DetailDto?)null);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext {{ TenantId = 10, CompanyId = 20 }});
        var handler = new Get{e}ByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new Get{e}ByIdQuery(99), CancellationToken.None);
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }}

    [Fact]
    public async Task List_ReturnsPagedResult()
    {{
        var readRepository = new Mock<I{e}ReadRepository>();
        readRepository.Setup(r => r.ListAsync(It.IsAny<AssetListQuery>(), It.IsAny<{e}ListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<{e}SummaryDto>([], 1, 25, 0));
        var handler = new List{f}QueryHandler(readRepository.Object);
        var response = await handler.Handle(new List{f}Query(new AssetListQuery(), new {e}ListFilters()), CancellationToken.None);
        response.Success.Should().BeTrue();
    }}
}}
""",
    )


def gen_simple_catalog(e: str, f: str, r: str, display: str, display_cap: str, controller: str) -> None:
    dbset = {"FgsAssetType": "FgsAssetTypes", "FgsAssetManufacturer": "FgsAssetManufacturers", "FgsAssetStatus": "FgsAssetStatuses"}[e]
    write(
        f"Fgs.Asset.Application/Abstractions/{f}/I{e}ReadRepository.cs",
        f"""using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Foundation.Paging;

namespace Fgs.Asset.Application.Abstractions.{f};

public interface I{e}ReadRepository
{{
    Task<{e}DetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<{e}SummaryDto>> ListAsync(AssetListQuery query, {e}ListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<{e}LookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, long? excludeId = null, CancellationToken cancellationToken = default);
}}
""",
    )
    write(
        f"Fgs.Asset.Application/Abstractions/{f}/I{e}WriteService.cs",
        f"""using Fgs.Asset.Application.Features.{f}.Dtos;

namespace Fgs.Asset.Application.Abstractions.{f};

public interface I{e}WriteService
{{
    Task<{e}DetailDto> CreateAsync({e}CreateDto dto, CancellationToken cancellationToken = default);
    Task<{e}DetailDto> UpdateAsync(long id, {e}UpdateDto dto, CancellationToken cancellationToken = default);
    Task<{e}DetailDto> PatchAsync(long id, {e}PatchDto dto, CancellationToken cancellationToken = default);
}}
""",
    )
    write(
        f"Fgs.Asset.Application/Features/{f}/Dtos/{e}Dtos.cs",
        f"""namespace Fgs.Asset.Application.Features.{f}.Dtos;

public sealed record {e}SummaryDto(long Id, string Code, string Name, string? Description, bool IsActive);
public sealed record {e}DetailDto(long Id, string Code, string Name, string? Description, bool IsActive);
public sealed record {e}LookupDto(long Id, string Code, string Name);
public sealed record {e}CreateDto(string Code, string Name, string? Description);
public sealed record {e}UpdateDto(string Code, string Name, string? Description);
public sealed record {e}PatchDto(string? Code, string? Name, string? Description, bool? IsActive);
public sealed record {e}ListFilters(string? Code = null, string? Name = null);
""",
    )
    write(
        f"Fgs.Asset.Application/Features/{f}/Validators/{e}Validators.cs",
        f"""using Fgs.Asset.Application.Abstractions.{f};
using Fgs.Asset.Application.Features.{f}.Commands.Create{e};
using Fgs.Asset.Application.Features.{f}.Commands.Patch{e};
using Fgs.Asset.Application.Features.{f}.Commands.Update{e};
using FluentValidation;

namespace Fgs.Asset.Application.Features.{f}.Validators;

public sealed class Create{e}CommandValidator : AbstractValidator<Create{e}Command>
{{
    public Create{e}CommandValidator(I{e}ReadRepository readRepository)
    {{
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, ct) => !await readRepository.ExistsByCodeAsync(code, null, ct)).WithMessage("A {display} with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.Description).MaximumLength(4000);
    }}
}}

public sealed class Update{e}CommandValidator : AbstractValidator<Update{e}Command>
{{
    public Update{e}CommandValidator(I{e}ReadRepository readRepository)
    {{
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, ct) => !await readRepository.ExistsByCodeAsync(code, command.Id, ct)).WithMessage("A {display} with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.Description).MaximumLength(4000);
    }}
}}

public sealed class Patch{e}CommandValidator : AbstractValidator<Patch{e}Command>
{{
    public Patch{e}CommandValidator(I{e}ReadRepository readRepository)
    {{
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(75).When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.").When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, ct) => !await readRepository.ExistsByCodeAsync(code!, command.Id, ct)).WithMessage("A {display} with this code already exists.").When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200).When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Description).MaximumLength(4000).When(x => x.Dto.Description is not null);
    }}
}}
""",
    )
    gen_commands_queries_controller(
        e, f, r, display_cap, controller,
        "        [FromQuery] string? code = null,\n        [FromQuery] string? name = null,\n",
        f"new {e}ListFilters(code, name)",
    )
    cols = '\\"Id\\", \\"Code\\", \\"Name\\", \\"Description\\", \\"IsActive\\"'
    write(
        f"Fgs.Asset.Infrastructure/{f}/{e}Sql.cs",
        f"""using Fgs.Foundation.Paging;

namespace Fgs.Asset.Infrastructure.{f};

internal static class {e}Sql
{{
    public const string Table = "asset.\\"{e}\\"";
    public const string SelectDetailColumns = "{cols}";
    public const string SelectSummaryColumns = SelectDetailColumns;
    public const string SelectLookupColumns = "\\"Id\\", \\"Code\\", \\"Name\\"";
    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase) {{ "Id", "IsActive", "Code", "Name", "Description" }};
    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {{
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy)) return $"ORDER BY \\"Id\\" {{dir}}";
        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \\"{{column}}\\" {{dir}}";
    }}
}}
""",
    )
    write(
        f"Fgs.Asset.Infrastructure/{f}/{e}DapperRows.cs",
        f"""using Fgs.Asset.Application.Features.{f}.Dtos;
namespace Fgs.Asset.Infrastructure.{f};
internal sealed class {e}SummaryRow {{ public long Id {{ get; set; }} public string Code {{ get; set; }} = null!; public string Name {{ get; set; }} = null!; public string? Description {{ get; set; }} public bool IsActive {{ get; set; }} public {e}SummaryDto ToDto() => new(Id, Code, Name, Description, IsActive); }}
internal sealed class {e}DetailRow {{ public long Id {{ get; set; }} public string Code {{ get; set; }} = null!; public string Name {{ get; set; }} = null!; public string? Description {{ get; set; }} public bool IsActive {{ get; set; }} public {e}DetailDto ToDto() => new(Id, Code, Name, Description, IsActive); }}
internal sealed class {e}LookupRow {{ public long Id {{ get; set; }} public string Code {{ get; set; }} = null!; public string Name {{ get; set; }} = null!; public {e}LookupDto ToDto() => new(Id, Code, Name); }}
""",
    )
    write(
        f"Fgs.Asset.Infrastructure/{f}/{e}ReadRepository.cs",
        textwrap.dedent(
            f"""
            using Dapper;
            using Fgs.Asset.Application.Abstractions.Persistence;
            using Fgs.Asset.Application.Abstractions.{f};
            using Fgs.Asset.Application.Common.AssetCrud;
            using Fgs.Asset.Application.Features.{f}.Dtos;
            using Fgs.Asset.Infrastructure.Common;
            using Fgs.Foundation.Paging;
            using Fgs.MultiTenancy;

            namespace Fgs.Asset.Infrastructure.{f};

            internal sealed class {e}ReadRepository : I{e}ReadRepository
            {{
                private readonly IAssetReadConnectionFactory _connectionFactory;
                private readonly ITenantContextAccessor _tenantContextAccessor;
                public {e}ReadRepository(IAssetReadConnectionFactory connectionFactory, ITenantContextAccessor tenantContextAccessor)
                {{ _connectionFactory = connectionFactory; _tenantContextAccessor = tenantContextAccessor; }}

                public async Task<{e}DetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
                {{
                    var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
                    var sql = $"SELECT {{{e}Sql.SelectDetailColumns}} FROM {{{e}Sql.Table}} WHERE \\"Id\\" = @Id AND \\"TenantId\\" = @TenantId AND \\"CompanyId\\" = @CompanyId";
                    await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
                    return (await connection.QueryFirstOrDefaultAsync<{e}DetailRow>(new CommandDefinition(sql, new {{ Id = id, TenantId = tenantId, CompanyId = companyId }}, cancellationToken: cancellationToken)))?.ToDto();
                }}

                public async Task<PagedResult<{e}SummaryDto>> ListAsync(AssetListQuery query, {e}ListFilters filters, CancellationToken cancellationToken = default)
                {{
                    var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
                    var paging = query.ToPagedQuery();
                    var page = Math.Max(1, paging.Page);
                    var pageSize = Math.Clamp(paging.PageSize, 1, 200);
                    var offset = (page - 1) * pageSize;
                    var where = new List<string> {{ "\\"TenantId\\" = @TenantId", "\\"CompanyId\\" = @CompanyId" }};
                    if (paging.IsActive.HasValue) where.Add("\\"IsActive\\" = @IsActive");
                    if (!string.IsNullOrWhiteSpace(filters.Code)) where.Add("\\"Code\\" = @Code");
                    if (!string.IsNullOrWhiteSpace(filters.Name)) where.Add("\\"Name\\" ILIKE @Name");
                    if (!string.IsNullOrWhiteSpace(paging.Search)) where.Add("(\\"Code\\" ILIKE @Search OR \\"Name\\" ILIKE @Search OR \\"Description\\" ILIKE @Search)");
                    var whereClause = string.Join(" AND ", where);
                    var sql = $"SELECT {{{e}Sql.SelectSummaryColumns}} FROM {{{e}Sql.Table}} WHERE {{whereClause}} {{{e}Sql.ResolveOrderBy(paging.SortBy, paging.SortDirection)}} LIMIT @PageSize OFFSET @Offset; SELECT COUNT(*) FROM {{{e}Sql.Table}} WHERE {{whereClause}};";
                    var parameters = new {{ TenantId = tenantId, CompanyId = companyId, IsActive = paging.IsActive, Code = filters.Code?.Trim().ToUpperInvariant(), Name = string.IsNullOrWhiteSpace(filters.Name) ? null : $"%{{filters.Name.Trim()}}%", Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{{paging.Search.Trim()}}%", PageSize = pageSize, Offset = offset }};
                    await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
                    await using var multi = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
                    var rows = (await multi.ReadAsync<{e}SummaryRow>()).ToList();
                    return new PagedResult<{e}SummaryDto>(rows.Select(r => r.ToDto()).ToList(), page, pageSize, await multi.ReadSingleAsync<int>());
                }}

                public async Task<IReadOnlyList<{e}LookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default)
                {{
                    var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
                    var activeFilter = activeOnly ? "AND \\"IsActive\\" = TRUE" : string.Empty;
                    var sql = $"SELECT {{{e}Sql.SelectLookupColumns}} FROM {{{e}Sql.Table}} WHERE \\"TenantId\\" = @TenantId AND \\"CompanyId\\" = @CompanyId {{activeFilter}} ORDER BY \\"Name\\" ASC";
                    await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
                    return (await connection.QueryAsync<{e}LookupRow>(new CommandDefinition(sql, new {{ TenantId = tenantId, CompanyId = companyId }}, cancellationToken: cancellationToken))).Select(r => r.ToDto()).ToList();
                }}

                public async Task<bool> ExistsByCodeAsync(string code, long? excludeId = null, CancellationToken cancellationToken = default)
                {{
                    var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
                    var exclude = excludeId.HasValue ? "AND \\"Id\\" <> @ExcludeId" : string.Empty;
                    var sql = $"SELECT EXISTS(SELECT 1 FROM {{{e}Sql.Table}} WHERE \\"TenantId\\" = @TenantId AND \\"CompanyId\\" = @CompanyId AND \\"Code\\" = @Code {{exclude}})";
                    await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
                    return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new {{ TenantId = tenantId, CompanyId = companyId, Code = code.Trim().ToUpperInvariant(), ExcludeId = excludeId }}, cancellationToken: cancellationToken));
                }}
            }}
            """
        ),
    )
    write(
        f"Fgs.Asset.Infrastructure/{f}/{e}WriteService.cs",
        f"""using Fgs.Asset.Application.Abstractions.{f};
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Asset.Domain.Entities;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Asset.Infrastructure.{f};

public sealed class {e}WriteService : I{e}WriteService
{{
    private readonly FgsAssetDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AssetEntityAuditHelper _auditHelper;
    public {e}WriteService(FgsAssetDbContext context, IUnitOfWork unitOfWork, AssetEntityAuditHelper auditHelper)
    {{ _context = context; _unitOfWork = unitOfWork; _auditHelper = auditHelper; }}

    public async Task<{e}DetailDto> CreateAsync({e}CreateDto dto, CancellationToken cancellationToken = default)
    {{
        var entity = new {e} {{ Code = dto.Code.Trim().ToUpperInvariant(), Name = dto.Name.Trim(), Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim() }};
        _auditHelper.StampForCreate(entity);
        await _context.{dbset}.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return Map(entity);
    }}

    public async Task<{e}DetailDto> UpdateAsync(long id, {e}UpdateDto dto, CancellationToken cancellationToken = default)
    {{
        var entity = await FindAsync(id, cancellationToken) ?? throw new KeyNotFoundException($"{display_cap} '{{id}}' was not found.");
        entity.Code = dto.Code.Trim().ToUpperInvariant();
        entity.Name = dto.Name.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return Map(entity);
    }}

    public async Task<{e}DetailDto> PatchAsync(long id, {e}PatchDto dto, CancellationToken cancellationToken = default)
    {{
        var entity = await FindAsync(id, cancellationToken) ?? throw new KeyNotFoundException($"{display_cap} '{{id}}' was not found.");
        if (dto.Code is not null) entity.Code = dto.Code.Trim().ToUpperInvariant();
        if (dto.Name is not null) entity.Name = dto.Name.Trim();
        if (dto.Description is not null) entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value;
        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return Map(entity);
    }}

    private async Task<{e}?> FindAsync(long id, CancellationToken cancellationToken) =>
        await _context.{dbset}.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {{
        try {{ await _unitOfWork.SaveChangesAsync(cancellationToken); }}
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true || ex.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true)
        {{ throw new InvalidOperationException("A {display} with the same code already exists.", ex); }}
    }}

    private static {e}DetailDto Map({e} entity) => new(entity.Id, entity.Code, entity.Name, entity.Description, entity.IsActive);
}}
""",
    )
    gen_simple_tests(e, f, r, display, display_cap)


# Import entity-specific generators
from gen_entities_remaining import (  # noqa: E402
    gen_all_remaining,
    update_dependency_injection,
    update_gateway,
)


if __name__ == "__main__":
    for item in [
        ("FgsAssetType", "AssetTypes", "assettype", "asset type", "Asset Type", "AssetTypeController"),
        ("FgsAssetManufacturer", "AssetManufacturers", "assetmanufacturer", "asset manufacturer", "Asset Manufacturer", "AssetManufacturerController"),
        ("FgsAssetStatus", "AssetStatuses", "assetstatus", "asset status", "Asset Status", "AssetStatusController"),
    ]:
        gen_simple_catalog(*item)
    gen_all_remaining(write, gen_commands_queries_controller, gen_simple_tests, CREATED)
    update_dependency_injection(write)
    update_gateway(GATEWAY, CREATED)
    manifest = os.path.join(ROOT, "scripts", "_generated_files.txt")
    with open(manifest, "w", encoding="utf-8") as f:
        f.write("\n".join(sorted(CREATED)))
    print(f"Generated {len(CREATED)} files")
