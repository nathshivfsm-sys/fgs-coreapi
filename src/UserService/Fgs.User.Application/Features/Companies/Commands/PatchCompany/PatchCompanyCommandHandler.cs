using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy.Constants;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.Security.Authorization;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Features.Companies.Dtos;
using Fgs.User.Application.Features.Companies.Queries.GetCompany;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Companies.Commands.PatchCompany;

public sealed class PatchCompanyCommandHandler(
    IUserWriteRepository<FgsTenantCompany> companyWriteRepository,
    IUserWriteRepository<FgsTenantCompanyCache> companyCacheWriteRepository,
    IUserWriteRepository<FgsLocation> locationWriteRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    ICacheService cache,
    IFgsUserContext userContext)
    : IRequestHandler<PatchCompanyCommand, ApiResponse<CompanyDetailDto>>
{
    public async Task<ApiResponse<CompanyDetailDto>> Handle(
        PatchCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var denied = AuthenticatedUserTenantScopeGuard.DenyCrossTenantCompanyAccess<CompanyDetailDto>(
            userContext,
            request.TenantId,
            request.CompanyId);
        if (denied is not null)
        {
            return denied;
        }

        ApiResponse<CompanyDetailDto>? failure = null;
        var now = DateTimeOffset.UtcNow;
        var actor = userContext.UserId?.ToString() ?? userContext.DisplayName;

        await unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                var company = await companyWriteRepository.FirstOrDefaultAsync(
                    c => c.TenantId == request.TenantId && c.CompanyNumber == request.CompanyId,
                    ct);

                if (company is null)
                {
                    failure = ApiResponse<CompanyDetailDto>.Fail(["Company not found."], ApiStatusCodes.NotFound);
                    return;
                }

                ApplyPatch(company, request.Dto, now, actor);
                await CompanyLocationUpdater.UpdateLocationsAsync(
                    locationWriteRepository,
                    company,
                    request.Dto.PhysicalAddress,
                    request.Dto.BillingAddress,
                    now,
                    actor,
                    ct);

                companyWriteRepository.Update(company);

                var companyCache = await companyCacheWriteRepository.FirstOrDefaultAsync(
                    c => c.TenantId == request.TenantId && c.CompanyId == request.CompanyId,
                    ct);
                if (companyCache is not null)
                {
                    companyCache.CompanyName = company.Name;
                    companyCache.CompanyCode = company.Code;
                    companyCache.IsActive = company.IsActive;
                    companyCache.UpdatedOn = now;
                    companyCacheWriteRepository.Update(companyCache);
                }

                await unitOfWork.SaveChangesAsync(ct);
            },
            cancellationToken);

        if (failure is not null)
        {
            return failure;
        }

        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(request.TenantId, request.CompanyId, "tenant-company"),
            cancellationToken);
        await cache.RemoveAsync(
            CacheKeys.Build(
                request.TenantId,
                TenantScopeConstants.PlatformCompanyId,
                "tenant-companies",
                "list"),
            cancellationToken);
        await cache.RemoveAsync(
            CacheKeys.Build(
                request.TenantId,
                request.CompanyId,
                "company-aggregate",
                request.CompanyId.ToString()),
            cancellationToken);

        return await mediator.Send(
            new GetCompanyQuery(request.TenantId, request.CompanyId),
            cancellationToken);
    }

    private static void ApplyPatch(
        FgsTenantCompany company,
        CompanyPatchDto dto,
        DateTimeOffset now,
        string? actor)
    {
        if (dto.Name is not null)
        {
            company.Name = dto.Name.Trim();
        }

        if (dto.LegalName is not null)
        {
            company.LegalName = TrimOrNull(dto.LegalName);
        }

        if (dto.Email is not null)
        {
            company.Email = TrimOrNull(dto.Email);
        }

        if (dto.PhoneNumber is not null)
        {
            company.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber)
                ? null
                : SignupPhoneNormalizer.ToStorageFormat(dto.PhoneNumber);
        }

        if (dto.Website is not null)
        {
            company.Website = TrimOrNull(dto.Website);
        }

        if (dto.TaxId is not null)
        {
            company.TaxId = TrimOrNull(dto.TaxId);
        }

        if (dto.CompanySize is not null)
        {
            company.CompanySize = TrimOrNull(dto.CompanySize);
        }

        if (dto.TimeZone is not null)
        {
            company.TimeZone = TrimOrNull(dto.TimeZone);
        }

        if (dto.IsActive.HasValue)
        {
            company.IsActive = dto.IsActive.Value;
        }

        company.UpdatedOn = now;
        company.UpdatedBy = actor;
    }

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
