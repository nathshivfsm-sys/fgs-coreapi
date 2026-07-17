using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiWebhooks.Commands.UpdateFgsApiWebhook;

public sealed record UpdateFgsApiWebhookCommand(long Id, FgsApiWebhookUpdateDto Dto)
    : IRequest<ApiResponse<FgsApiWebhookDetailDto>>;
