#!/usr/bin/env python3
"""Generate Asset Service catalog CRUD files."""
import os

ROOT = r"c:\SourceCode\FGS\src\AssetService"

def write(path, content):
    full = os.path.join(ROOT, path.replace("/", os.sep))
    os.makedirs(os.path.dirname(full), exist_ok=True)
    with open(full, "w", encoding="utf-8", newline="\n") as f:
        f.write(content.rstrip() + "\n")

def gen_simple(entity, feature, route, display, display_cap, controller):
    e, f, r = entity, feature, route
    dbset = {
        "FgsAssetType": "FgsAssetTypes",
        "FgsAssetManufacturer": "FgsAssetManufacturers",
        "FgsAssetStatus": "FgsAssetStatuses",
    }[e]

    write(f"Fgs.Asset.Application/Abstractions/{f}/IFgs{e}ReadRepository.cs", f"""using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Foundation.Paging;

namespace Fgs.Asset.Application.Abstractions.{f};

public interface IFgs{e}ReadRepository
{{
    Task<Fgs{e}DetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<Fgs{e}SummaryDto>> ListAsync(
        AssetListQuery query,
        Fgs{e}ListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Fgs{e}LookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(
        string code,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}}
""")

    write(f"Fgs.Asset.Application/Abstractions/{f}/IFgs{e}WriteService.cs", f"""using Fgs.Asset.Application.Features.{f}.Dtos;

namespace Fgs.Asset.Application.Abstractions.{f};

public interface IFgs{e}WriteService
{{
    Task<Fgs{e}DetailDto> CreateAsync(Fgs{e}CreateDto dto, CancellationToken cancellationToken = default);
    Task<Fgs{e}DetailDto> UpdateAsync(long id, Fgs{e}UpdateDto dto, CancellationToken cancellationToken = default);
    Task<Fgs{e}DetailDto> PatchAsync(long id, Fgs{e}PatchDto dto, CancellationToken cancellationToken = default);
}}
""")

    write(f"Fgs.Asset.Application/Features/{f}/Dtos/Fgs{e}Dtos.cs", f"""namespace Fgs.Asset.Application.Features.{f}.Dtos;

public sealed record Fgs{e}SummaryDto(
    long Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive);

public sealed record Fgs{e}DetailDto(
    long Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive);

public sealed record Fgs{e}LookupDto(
    long Id,
    string Code,
    string Name);

public sealed record Fgs{e}CreateDto(
    string Code,
    string Name,
    string? Description);

public sealed record Fgs{e}UpdateDto(
    string Code,
    string Name,
    string? Description);

public sealed record Fgs{e}PatchDto(
    string? Code,
    string? Name,
    string? Description,
    bool? IsActive);

public sealed record Fgs{e}ListFilters(
    string? Code = null,
    string? Name = null);
""")

    write(f"Fgs.Asset.Application/Features/{f}/Validators/Fgs{e}Validators.cs", f"""using Fgs.Asset.Application.Abstractions.{f};
using Fgs.Asset.Application.Features.{f}.Commands.CreateFgs{e};
using Fgs.Asset.Application.Features.{f}.Commands.PatchFgs{e};
using Fgs.Asset.Application.Features.{f}.Commands.UpdateFgs{e};
using FluentValidation;

namespace Fgs.Asset.Application.Features.{f}.Validators;

public sealed class CreateFgs{e}CommandValidator : AbstractValidator<CreateFgs{e}Command>
{{
    public CreateFgs{e}CommandValidator(IFgs{e}ReadRepository readRepository)
    {{
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.Code)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, null, cancellationToken))
            .WithMessage("A {display} with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.Description).MaximumLength(4000);
    }}
}}

public sealed class UpdateFgs{e}CommandValidator : AbstractValidator<UpdateFgs{e}Command>
{{
    public UpdateFgs{e}CommandValidator(IFgs{e}ReadRepository readRepository)
    {{
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.Code)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A {display} with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.Description).MaximumLength(4000);
    }}
}}

public sealed class PatchFgs{e}CommandValidator : AbstractValidator<PatchFgs{e}Command>
{{
    public PatchFgs{e}CommandValidator(IFgs{e}ReadRepository readRepository)
    {{
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(75).When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Code)
            .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("Code must be uppercase.")
            .When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Code)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A {display} with this code already exists.")
            .When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200).When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Description).MaximumLength(4000).When(x => x.Dto.Description is not null);
    }}
}}
""")

    for cmd in ["Create", "Update", "Patch"]:
        if cmd == "Create":
            sig = f"Fgs{e}CreateDto Dto"
            call = "request.Dto"
            ret = f"return ApiResponse<Fgs{e}DetailDto>.Ok(result, ApiStatusCodes.Created);"
            log = f'logger.LogInformation("Created {display} {{Id}} with code {{Code}}", result.Id, result.Code);'
        else:
            sig = f"long Id, Fgs{e}{cmd}Dto Dto"
            call = "request.Id, request.Dto"
            ret = f"return ApiResponse<Fgs{e}DetailDto>.Ok(result);"
            log = f'logger.LogInformation("{cmd}ed {display} {{Id}}", result.Id);'
        method = cmd.lower() + "Async"
        write(f"Fgs.Asset.Application/Features/{f}/Commands/{cmd}Fgs{e}/{cmd}Fgs{e}Command.cs", f"""using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.{f}.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.{f}.Commands.{cmd}Fgs{e};

public sealed record {cmd}Fgs{e}Command({sig})
    : IRequest<ApiResponse<Fgs{e}DetailDto>>;
""")
        write(f"Fgs.Asset.Application/Features/{f}/Commands/{cmd}Fgs{e}/{cmd}Fgs{e}CommandHandler.cs", f"""using Fgs.Asset.Application.Abstractions.{f};
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Asset.Application.Features.{f}.Commands.{cmd}Fgs{e};

public sealed class {cmd}Fgs{e}CommandHandler(
    IFgs{e}WriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<{cmd}Fgs{e}CommandHandler> logger)
    : IRequestHandler<{cmd}Fgs{e}Command, ApiResponse<Fgs{e}DetailDto>>
{{
    public async Task<ApiResponse<Fgs{e}DetailDto>> Handle(
        {cmd}Fgs{e}Command request,
        CancellationToken cancellationToken)
    {{
        var result = await writeService.{method}({call}, cancellationToken);
        {log}
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "{r}"),
            cancellationToken);
        {ret}
    }}
}}
""")

    # Queries, infra, controller, tests - continue in part 2
    print(f"Generated application layer for {e}")

if __name__ == "__main__":
    for item in [
        ("FgsAssetType", "AssetTypes", "assettype", "asset type", "Asset Type", "AssetTypeController"),
        ("FgsAssetManufacturer", "AssetManufacturers", "assetmanufacturer", "asset manufacturer", "Asset Manufacturer", "AssetManufacturerController"),
        ("FgsAssetStatus", "AssetStatuses", "assetstatus", "asset status", "Asset Status", "AssetStatusController"),
    ]:
        gen_simple(*item)
    print("Part 1 complete")
