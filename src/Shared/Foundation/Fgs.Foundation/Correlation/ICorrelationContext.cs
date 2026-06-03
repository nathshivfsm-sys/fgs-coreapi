namespace Fgs.Foundation.Correlation;

public interface ICorrelationContext
{
    Guid GetCorrelationId();
}
