namespace Fgs.User.Application.Abstractions.Credentials;

public interface ICorrelationContext
{
    Guid GetCorrelationId();
}
