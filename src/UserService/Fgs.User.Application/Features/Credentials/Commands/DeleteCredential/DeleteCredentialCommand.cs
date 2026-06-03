using Fgs.Foundation.Result;
using Fgs.User.Domain.Enums;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Commands.DeleteCredential;

public sealed record DeleteCredentialCommand(
    CredentialScope Scope,
    string Id) : IRequest<ApiResponse<object>>;
