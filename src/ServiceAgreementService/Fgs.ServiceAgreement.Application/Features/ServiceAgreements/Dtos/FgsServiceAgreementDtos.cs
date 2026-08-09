namespace Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Dtos;

public sealed record FgsServiceAgreementSummaryDto(
    long Id,
    string AgreementNumber,
    string Name,
    long CustomerId,
    long CustomerLocationId,
    DateOnly StartDate,
    DateOnly EndDate,
    short ServiceAgreementStatusId,
    decimal ContractAmount);

public sealed record FgsServiceAgreementDetailDto(
    long Id,
    string AgreementNumber,
    long CustomerId,
    long CustomerLocationId,
    long? EstimateId,
    string Name,
    string? Description,
    long Break1Id,
    long Break2Id,
    long JobTypeId,
    DateOnly StartDate,
    DateOnly EndDate,
    short ServiceAgreementStatusId,
    short VisitFrequencyId,
    short BillingFrequencyId,
    decimal ContractAmount,
    decimal LaborDiscountPercent,
    decimal MaterialDiscountPercent,
    bool AutoRenew,
    long? RenewedByServiceAgreementId,
    DateOnly? SoldDate,
    long? SoldByEmployeeId,
    DateTimeOffset? ActivatedOn,
    DateTimeOffset? CancelledOn,
    string? ExternalEntityId,
    string? ExternalVersion);

public sealed record FgsServiceAgreementCreateDto(
    string AgreementNumber,
    long CustomerId,
    long CustomerLocationId,
    long? EstimateId,
    string Name,
    string? Description,
    long Break1Id,
    long Break2Id,
    long JobTypeId,
    DateOnly StartDate,
    DateOnly EndDate,
    short ServiceAgreementStatusId,
    short VisitFrequencyId,
    short BillingFrequencyId,
    decimal ContractAmount,
    decimal LaborDiscountPercent,
    decimal MaterialDiscountPercent,
    bool AutoRenew,
    DateOnly? SoldDate,
    long? SoldByEmployeeId,
    string? ExternalEntityId,
    string? ExternalVersion);

public sealed record FgsServiceAgreementListFilters(
    string? AgreementNumber = null,
    long? CustomerId = null,
    short? ServiceAgreementStatusId = null);
