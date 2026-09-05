using Fgs.User.Application.Common.Locations;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Domain.Entities;
using Fgs.User.Application.Abstractions.Persistence;

namespace Fgs.User.Application.Features.Companies;

internal static class CompanyLocationUpdater
{
    public static async Task UpdateLocationsAsync(
        IUserWriteRepository<FgsLocation> locationWriteRepository,
        FgsTenantCompany company,
        LocationWriteDto? physicalAddress,
        LocationWriteDto? billingAddress,
        DateTimeOffset now,
        string? actor,
        CancellationToken cancellationToken)
    {
        if (physicalAddress is null)
        {
            return;
        }

        FgsLocation physicalLocation;

        if (company.PhysicalLocationId.HasValue)
        {
            physicalLocation = await locationWriteRepository.GetByIdAsync(
                                   company.PhysicalLocationId.Value,
                                   cancellationToken)
                               ?? throw new InvalidOperationException("Physical location not found.");
            LocationMapper.ApplyWriteDto(physicalLocation, physicalAddress, now);
            physicalLocation.UpdatedBy = actor;
            locationWriteRepository.Update(physicalLocation);
        }
        else
        {
            physicalLocation = new FgsLocation
            {
                Id = Guid.NewGuid(),
                TenantId = company.TenantId,
                CompanyId = company.CompanyNumber,
                MasterEntityTypeId = SignupConstants.TenantCompanyMasterEntityTypeId,
                IsActive = true,
                CreatedOn = now,
                CreatedBy = actor
            };
            LocationMapper.ApplyWriteDto(physicalLocation, physicalAddress, now);
            await locationWriteRepository.AddAsync(physicalLocation, cancellationToken);
            company.PhysicalLocationId = physicalLocation.Id;
        }

        if (billingAddress is null)
        {
            company.BillingLocationId = physicalLocation.Id;
            return;
        }

        if (company.BillingLocationId.HasValue && company.BillingLocationId != company.PhysicalLocationId)
        {
            var billingLocation = await locationWriteRepository.GetByIdAsync(
                                      company.BillingLocationId.Value,
                                      cancellationToken)
                                  ?? throw new InvalidOperationException("Billing location not found.");
            LocationMapper.ApplyWriteDto(billingLocation, billingAddress, now);
            billingLocation.UpdatedBy = actor;
            locationWriteRepository.Update(billingLocation);
            return;
        }

        if (company.BillingLocationId == company.PhysicalLocationId
            || !company.BillingLocationId.HasValue)
        {
            var billingLocation = new FgsLocation
            {
                Id = Guid.NewGuid(),
                TenantId = company.TenantId,
                CompanyId = company.CompanyNumber,
                MasterEntityTypeId = SignupConstants.TenantCompanyMasterEntityTypeId,
                IsActive = true,
                CreatedOn = now,
                CreatedBy = actor
            };
            LocationMapper.ApplyWriteDto(billingLocation, billingAddress, now);
            await locationWriteRepository.AddAsync(billingLocation, cancellationToken);
            company.BillingLocationId = billingLocation.Id;
        }
    }
}
