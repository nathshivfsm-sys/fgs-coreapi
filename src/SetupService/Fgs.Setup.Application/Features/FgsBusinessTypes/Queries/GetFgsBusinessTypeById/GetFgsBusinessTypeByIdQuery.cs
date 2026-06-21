using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Queries.GetFgsBusinessTypeById;

public sealed record GetFgsBusinessTypeByIdQuery(long Id)
    : IRequest<ApiResponse<FgsBusinessTypeDetailDto>>;
