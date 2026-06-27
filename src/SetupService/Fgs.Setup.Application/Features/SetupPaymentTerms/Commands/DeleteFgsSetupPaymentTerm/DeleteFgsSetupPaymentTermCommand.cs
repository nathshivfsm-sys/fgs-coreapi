using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.DeleteFgsSetupPaymentTerm;

public sealed record DeleteFgsSetupPaymentTermCommand(long Id)
    : IRequest<ApiResponse<FgsSetupPaymentTermDetailDto>>;
