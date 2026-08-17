using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy.Constants;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.Security.Authorization;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common.Locations;
using Fgs.User.Application.Features.Companies.Dtos;
using Fgs.User.Application.Features.Companies.Queries.GetCompany;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Companies.Commands.UpdateCompany;

public sealed class UpdateCompanyCommandHandler(
    IUserWriteRepository<FgsTenantCompany> companyWriteRepository,
    IUserWriteRepository<FgsTenantCompanyCache> companyCacheWriteRepository,
    IUserWriteRepository<FgsLocation> locationWriteRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    ICacheService cache,
    IFgsUserContext userContext)
    : IRequestHandler<UpdateCompanyCommand, ApiResponse<CompanyDetailDto>>
{
    public async Task<ApiResponse<CompanyDetailDto>> Handle(
        UpdateCompanyCommand request,
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

                ApplyUpdate(company, request.Dto, now, actor);
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

        await InvalidateCachesAsync(request.TenantId, request.CompanyId, cancellationToken);
        return await mediator.Send(
            new GetCompanyQuery(request.TenantId, request.CompanyId),
            cancellationToken);
    }

    private static void ApplyUpdate(
        FgsTenantCompany company,
        CompanyUpdateDto dto,
        DateTimeOffset now,
        string? actor)
    {
        company.Name = dto.Name.Trim();
        company.LegalName = TrimOrNull(dto.LegalName);
        company.Email = TrimOrNull(dto.Email);
        company.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber)
            ? null
            : SignupPhoneNormalizer.ToStorageFormat(dto.PhoneNumber);
        company.Website = TrimOrNull(dto.Website);
        company.TaxId = TrimOrNull(dto.TaxId);
        company.CompanySize = TrimOrNull(dto.CompanySize);
        company.TimeZone = TrimOrNull(dto.TimeZone);
        company.IsActive = dto.IsActive;
        company.UpdatedOn = now;
        company.UpdatedBy = actor;
    }

    private async Task InvalidateCachesAsync(long tenantId, long companyId, CancellationToken cancellationToken)
    {
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantId, companyId, "tenant-company"),
            cancellationToken);
        await cache.RemoveAsync(
            CacheKeys.Build(
                tenantId,
                TenantScopeConstants.PlatformCompanyId,
                "tenant-companies",
                "list"),
            cancellationToken);
        await cache.RemoveAsync(
            CacheKeys.Build(
                tenantId,
                companyId,
                "company-aggregate",
                companyId.ToString()),
            cancellationToken);
    }

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
