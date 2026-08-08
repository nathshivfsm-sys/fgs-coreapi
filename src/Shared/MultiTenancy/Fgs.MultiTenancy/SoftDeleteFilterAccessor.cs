namespace Fgs.MultiTenancy;

/// <summary>
/// Controls whether EF soft-delete (<c>IsActive</c>) filters are applied.
/// Uses <see cref="AsyncLocal{T}"/> so any accessor instance shares request scope state.
/// </summary>
public interface ISoftDeleteFilterAccessor
{
    bool IsEnabled { get; set; }
}

public sealed class SoftDeleteFilterAccessor : ISoftDeleteFilterAccessor
{
    private static readonly AsyncLocal<bool?> Override = new();

    public bool IsEnabled
    {
        get => Override.Value ?? true;
        set => Override.Value = value;
    }
}
