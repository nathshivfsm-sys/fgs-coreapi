using Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Dtos;

namespace Fgs.ServiceAgreement.Application.Abstractions.ServiceAgreements;

public interface IFgsServiceAgreementWriteService
{
    Task<FgsServiceAgreementDetailDto> CreateAsync(
        FgsServiceAgreementCreateDto dto,
        CancellationToken cancellationToken = default);
}
