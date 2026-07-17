using Fgs.Contracts.Api;
using Fgs.User.Application.Features.DataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccesses.Queries.GetFgsDataAccessById;

public sealed record GetFgsDataAccessByIdQuery(long Id) : IRequest<ApiResponse<FgsDataAccessDetailDto>>;
