using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Application.Settings;

/// <summary>
/// One settings snapshot with what the running app derives from it: the play command for each sound the
/// snapshot references, carrying the managed copy it plays. Immutable; <see cref="ActiveSettings.Publish"/>
/// replaces it as a whole.
/// </summary>
/// <remarks>
/// A command keeps the copy it was built with, so a request queued before a newer configuration was
/// published still plays, and reports, the copy of the configuration that built its binding.
/// </remarks>
public sealed class ActiveConfiguration
{
    private readonly Dictionary<SoundId, PlaySoundCommand> commands = [];

    internal ActiveConfiguration(SettingsSnapshot snapshot, Func<SoundReference, string> managedPath)
    {
        Snapshot = snapshot;
        foreach (var sound in snapshot.Slots.Select(slot => slot.Action).OfType<SoundAction>().Select(action => action.Sound))
        {
            var id = IdOf(sound);
            if (!commands.ContainsKey(id))
            {
                commands.Add(id, new PlaySoundCommand(id, managedPath(sound)));
            }
        }
    }

    public SettingsSnapshot Snapshot { get; }

    /// <summary>The play command for a sound of <see cref="Snapshot"/>, for building its shortcut bindings.</summary>
    /// <exception cref="ArgumentException"><see cref="Snapshot"/> does not reference <paramref name="sound"/>.</exception>
    public PlaySoundCommand CommandFor(SoundReference sound)
    {
        ArgumentNullException.ThrowIfNull(sound);
        return commands.TryGetValue(IdOf(sound), out var command)
            ? command
            : throw new ArgumentException($"Settings do not reference sound '{sound.LibraryFileName}'.", nameof(sound));
    }

    private static SoundId IdOf(SoundReference sound) => new(sound.LibraryFileName);
}

/// <summary>
/// The configuration the running app uses. Shortcut bindings and the drawing style read <see cref="Current"/>;
/// sound playback and sound failure reporting use the managed copy carried by each bound command, so they see
/// the configuration that built the binding.
/// </summary>
/// <remarks>
/// <see cref="Current"/> may be read from any thread. An operation that needs several values reads it once
/// and keeps the result, so it never mixes two configurations.
/// </remarks>
public sealed class ActiveSettings
{
    private readonly Func<SoundReference, string> managedPath;
    private ActiveConfiguration current;

    /// <param name="snapshot">The configuration to start with, for example <see cref="StartupSettings.Active"/>.</param>
    /// <param name="managedPath">The absolute path of a sound's managed copy in the sound library.</param>
    public ActiveSettings(SettingsSnapshot snapshot, Func<SoundReference, string> managedPath)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(managedPath);
        this.managedPath = managedPath;
        current = new ActiveConfiguration(snapshot, managedPath);
    }

    public ActiveConfiguration Current => Volatile.Read(ref current);

    /// <summary>
    /// Makes <paramref name="snapshot"/> the active configuration in one write: readers see the previous
    /// configuration or the new one, never a mix. Call only after <paramref name="snapshot"/> was persisted.
    /// </summary>
    /// <returns>The new configuration, to rebuild shortcut bindings from.</returns>
    public ActiveConfiguration Publish(SettingsSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        var published = new ActiveConfiguration(snapshot, managedPath);
        Volatile.Write(ref current, published);
        return published;
    }
}
