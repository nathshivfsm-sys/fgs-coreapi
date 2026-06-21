using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.CreateFgsSetupPaymentTerm;

public sealed record CreateFgsSetupPaymentTermCommand(FgsSetupPaymentTermCreateDto Dto)
    : IRequest<ApiResponse<FgsSetupPaymentTermDetailDto>>;
