using Fgs.Persistence.Abstractions;
using Fgs.ServiceAgreement.Application.Abstractions.ServiceAgreements;
using Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Dtos;
using Fgs.ServiceAgreement.Domain.Entities;
using Fgs.ServiceAgreement.Infrastructure.Common;
using Fgs.ServiceAgreement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.ServiceAgreement.Infrastructure.Persistence.ServiceAgreements;

public sealed class FgsServiceAgreementWriteService : IFgsServiceAgreementWriteService
{
    private readonly FgsServiceAgreementDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ServiceAgreementEntityAuditHelper _auditHelper;

    public FgsServiceAgreementWriteService(
        FgsServiceAgreementDbContext context,
        IUnitOfWork unitOfWork,
        ServiceAgreementEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsServiceAgreementDetailDto> CreateAsync(
        FgsServiceAgreementCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsServiceAgreement
        {
            AgreementNumber = NormalizeAgreementNumber(dto.AgreementNumber),
            CustomerId = dto.CustomerId,
            CustomerLocationId = dto.CustomerLocationId,
            EstimateId = dto.EstimateId,
            Name = dto.Name.Trim(),
            Description = TrimOrNull(dto.Description),
            Break1Id = dto.Break1Id,
            Break2Id = dto.Break2Id,
            JobTypeId = dto.JobTypeId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            ServiceAgreementStatusId = dto.ServiceAgreementStatusId,
            VisitFrequencyId = dto.VisitFrequencyId,
            BillingFrequencyId = dto.BillingFrequencyId,
            ContractAmount = dto.ContractAmount,
            LaborDiscountPercent = dto.LaborDiscountPercent,
            MaterialDiscountPercent = dto.MaterialDiscountPercent,
            AutoRenew = dto.AutoRenew,
            SoldDate = dto.SoldDate,
            SoldByEmployeeId = dto.SoldByEmployeeId,
            ExternalEntityId = TrimOrNull(dto.ExternalEntityId),
            ExternalVersion = TrimOrNull(dto.ExternalVersion)
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsServiceAgreements.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return FgsServiceAgreementReadRepository.MapToDetail(entity);
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A service agreement with the same number already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeAgreementNumber(string agreementNumber) =>
        agreementNumber.Trim().ToUpperInvariant();

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
