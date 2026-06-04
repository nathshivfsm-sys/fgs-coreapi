using Fgs.Contracts.Api;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Commands.DeleteCredential;

public sealed record DeleteCredentialCommand(
    CredentialScope Scope,
    string Id) : IRequest<ApiResponse<object>>;

