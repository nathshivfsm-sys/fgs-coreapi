using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.UpdateFgsSetupPaymentTerm;

public sealed record UpdateFgsSetupPaymentTermCommand(long Id, FgsSetupPaymentTermUpdateDto Dto)
    : IRequest<ApiResponse<FgsSetupPaymentTermDetailDto>>;
