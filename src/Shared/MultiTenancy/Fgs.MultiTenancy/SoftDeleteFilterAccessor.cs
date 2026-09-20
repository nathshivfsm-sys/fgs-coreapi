namespace Fgs.MultiTenancy;

/// <summary>
/// Controls whether EF soft-delete (<c>IsActive</c>) filters are applied.
/// Uses <see cref="AsyncLocal{T}"/> so any accessor instance shares request scope state.
/// </summary>
public interface ISoftDeleteFilterAccessor
{
    bool IsEnabled { get; set; }

    /// <summary>
    /// Temporarily disables the soft-delete filter (keeps tenant filters).
    /// Dispose to restore the previous value.
    /// </summary>
    IDisposable Suppress();
}

public sealed class SoftDeleteFilterAccessor : ISoftDeleteFilterAccessor
{
    private static readonly AsyncLocal<bool?> Override = new();

    public bool IsEnabled
    {
        get => Override.Value ?? true;
        set => Override.Value = value;
    }

    public IDisposable Suppress()
    {
        var previous = Override.Value;
        Override.Value = false;
        return new SoftDeleteFilterSuppression(previous);
    }

    /// <summary>
    /// Suppress soft-delete filtering for the current async context without resolving DI.
    /// Safe because enablement is stored in a static <see cref="AsyncLocal{T}"/>.
    /// </summary>
    public static IDisposable BeginSuppress() => new SoftDeleteFilterAccessor().Suppress();

    private sealed class SoftDeleteFilterSuppression(bool? previous) : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            Override.Value = previous;
            _disposed = true;
        }
    }
}
