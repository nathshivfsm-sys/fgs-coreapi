using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.NonWorkingDates.Commands.PatchFgsNonWorkingDate;

public sealed record PatchFgsNonWorkingDateCommand(long Id, FgsNonWorkingDatePatchDto Dto)
    : IRequest<ApiResponse<FgsNonWorkingDateDetailDto>>;
