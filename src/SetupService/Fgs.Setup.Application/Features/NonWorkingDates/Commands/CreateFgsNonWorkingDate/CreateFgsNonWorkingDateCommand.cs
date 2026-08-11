using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.NonWorkingDates.Commands.CreateFgsNonWorkingDate;

public sealed record CreateFgsNonWorkingDateCommand(FgsNonWorkingDateCreateDto Dto)
    : IRequest<ApiResponse<FgsNonWorkingDateDetailDto>>;
