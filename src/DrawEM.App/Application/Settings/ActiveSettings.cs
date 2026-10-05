using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;

namespace DrawEM.App.Application.Settings;

/// <summary>
/// The settings snapshot the running app uses, and the play commands for its sounds. Shortcut bindings are
/// built from one read of <see cref="Current"/>, so a binding never mixes two configurations.
/// </summary>
/// <remarks>
/// <see cref="Current"/> may be read from any thread. A command carries the path of its sound's managed copy.
/// Copy names are content hashes, so the command plays the same file under every configuration that
/// references the sound, including one published while the command waits on the sound thread.
/// </remarks>
public sealed class ActiveSettings
{
    private readonly Func<SoundReference, string> managedPath;
    private SettingsSnapshot current;

    /// <param name="snapshot">The configuration to start with, for example <see cref="StartupSettings.Active"/>.</param>
    /// <param name="managedPath">The absolute path of a sound's managed copy in the sound library.</param>
    public ActiveSettings(SettingsSnapshot snapshot, Func<SoundReference, string> managedPath)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(managedPath);
        this.managedPath = managedPath;
        current = snapshot;
    }

    public SettingsSnapshot Current => Volatile.Read(ref current);

    /// <summary>Makes <paramref name="snapshot"/> current in one write. Call only after it was persisted.</summary>
    public void Publish(SettingsSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        Volatile.Write(ref current, snapshot);
    }

    /// <summary>The play command for <paramref name="sound"/>, for building shortcut bindings.</summary>
    public PlaySoundCommand CommandFor(SoundReference sound)
    {
        ArgumentNullException.ThrowIfNull(sound);
        return new PlaySoundCommand(new SoundId(sound.LibraryFileName), managedPath(sound));
    }
}
