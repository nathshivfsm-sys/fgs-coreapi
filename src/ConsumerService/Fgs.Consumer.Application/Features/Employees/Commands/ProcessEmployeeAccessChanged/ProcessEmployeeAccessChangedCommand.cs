using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Consumer;
using MediatR;

namespace Fgs.Consumer.Application.Features.Employees.Commands.ProcessEmployeeAccessChanged;

public sealed record ProcessEmployeeAccessChangedCommand(
    EmployeeAccessChangedEvent Event,
    ConsumerMessageContext Context) : IRequest;
