using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Commands.CreateFgsBusinessType;

public sealed record CreateFgsBusinessTypeCommand(FgsBusinessTypeCreateDto Dto)
    : IRequest<ApiResponse<FgsBusinessTypeDetailDto>>;
