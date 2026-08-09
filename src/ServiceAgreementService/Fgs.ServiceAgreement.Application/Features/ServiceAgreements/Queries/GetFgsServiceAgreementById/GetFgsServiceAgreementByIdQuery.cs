using Fgs.Contracts.Api;
using Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Dtos;
using MediatR;

namespace Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Queries.GetFgsServiceAgreementById;

public sealed record GetFgsServiceAgreementByIdQuery(long Id)
    : IRequest<ApiResponse<FgsServiceAgreementDetailDto>>;
