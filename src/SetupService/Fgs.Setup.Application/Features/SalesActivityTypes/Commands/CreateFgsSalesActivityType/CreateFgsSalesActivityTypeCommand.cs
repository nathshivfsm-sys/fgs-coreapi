using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Commands.CreateFgsSalesActivityType;

public sealed record CreateFgsSalesActivityTypeCommand(FgsSalesActivityTypeCreateDto Dto)
    : IRequest<ApiResponse<FgsSalesActivityTypeDetailDto>>;
