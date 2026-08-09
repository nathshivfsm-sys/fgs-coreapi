using Fgs.Contracts.Api;
using Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Dtos;
using MediatR;

namespace Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Commands.CreateFgsServiceAgreement;

public sealed record CreateFgsServiceAgreementCommand(FgsServiceAgreementCreateDto Dto)
    : IRequest<ApiResponse<FgsServiceAgreementDetailDto>>;
