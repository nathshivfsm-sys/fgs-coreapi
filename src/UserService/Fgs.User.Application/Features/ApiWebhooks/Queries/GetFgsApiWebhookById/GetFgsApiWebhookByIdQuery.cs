using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiWebhooks.Queries.GetFgsApiWebhookById;

public sealed record GetFgsApiWebhookByIdQuery(long Id) : IRequest<ApiResponse<FgsApiWebhookDetailDto>>;
