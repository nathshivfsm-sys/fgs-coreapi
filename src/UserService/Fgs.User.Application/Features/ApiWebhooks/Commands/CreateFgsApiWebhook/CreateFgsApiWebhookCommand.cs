using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiWebhooks.Commands.CreateFgsApiWebhook;

public sealed record CreateFgsApiWebhookCommand(FgsApiWebhookCreateDto Dto)
    : IRequest<ApiResponse<FgsApiWebhookDetailDto>>;
