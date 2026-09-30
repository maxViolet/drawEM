using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Application.Settings;

/// <summary>Outcome of reading saved settings.</summary>
public abstract record SettingsLoadResult
{
    private SettingsLoadResult()
    {
    }

    /// <summary>Saved settings were read and validated.</summary>
    public sealed record Loaded(SettingsSnapshot Snapshot) : SettingsLoadResult;

    /// <summary>No settings have been saved yet.</summary>
    public sealed record Missing : SettingsLoadResult;

    /// <summary>
    /// Saved settings exist but cannot be used: unreadable, malformed, invalid, or an unsupported schema
    /// version. The saved data is left in place. <paramref name="RecoveryCopy"/> names a retained copy of
    /// the bytes, or is <c>null</c> when no copy could be made.
    /// </summary>
    public sealed record Unreadable(string Reason, string? RecoveryCopy) : SettingsLoadResult;
}

/// <summary>Persists one settings snapshot for the current user.</summary>
public interface ISettingsStore
{
    SettingsLoadResult Load();

    /// <summary>
    /// Durably replaces the saved settings. On failure the previously saved settings stay intact.
    /// </summary>
    /// <exception cref="SettingsStoreException">The settings could not be saved.</exception>
    void Save(SettingsSnapshot snapshot);
}

/// <summary>Settings could not be saved. The previous saved and active settings are unchanged.</summary>
public sealed class SettingsStoreException : Exception
{
    public SettingsStoreException(string message)
        : base(message)
    {
    }

    public SettingsStoreException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
