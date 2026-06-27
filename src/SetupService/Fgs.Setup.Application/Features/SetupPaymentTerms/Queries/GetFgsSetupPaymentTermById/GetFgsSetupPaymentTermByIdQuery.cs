using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Queries.GetFgsSetupPaymentTermById;

public sealed record GetFgsSetupPaymentTermByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSetupPaymentTermDetailDto>>;
