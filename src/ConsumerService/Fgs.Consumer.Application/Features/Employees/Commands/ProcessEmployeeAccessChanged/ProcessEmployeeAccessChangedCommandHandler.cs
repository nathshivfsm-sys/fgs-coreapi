using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.Consumer.Application.Features.Employees.Commands.ProcessEmployeeAccessChanged;

public sealed class ProcessEmployeeAccessChangedCommandHandler(IUserInternalUsersClient userClient)
    : IRequestHandler<ProcessEmployeeAccessChangedCommand>
{
    public async Task Handle(
        ProcessEmployeeAccessChangedCommand request,
        CancellationToken cancellationToken)
    {
        var evt = request.Event;
        var response = await userClient.SetUserAccessAsync(
            evt.UserId,
            new SetUserAccessRequest(evt.IsActive),
            evt.TenantId.ToString(),
            evt.CompanyId.ToString(),
            cancellationToken);

        if (!response.Success)
        {
            var message = response.Errors.Count > 0
                ? string.Join("; ", response.Errors)
                : "User access update failed.";
            throw new InvalidOperationException(message);
        }
    }
}
