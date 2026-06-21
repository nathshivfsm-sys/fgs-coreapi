using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.PatchFgsSetupPaymentTerm;

public sealed record PatchFgsSetupPaymentTermCommand(long Id, FgsSetupPaymentTermPatchDto Dto)
    : IRequest<ApiResponse<FgsSetupPaymentTermDetailDto>>;
