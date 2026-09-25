using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Owns the slot-to-sound mapping. The only resolution path from a shortcut slot to a play command,
/// and from a sound to its file path.
/// </summary>
public sealed class SoundConfiguration
{
    private readonly Dictionary<SoundSlot, SoundAssignment> assignments;
    private readonly Dictionary<SoundId, string> paths = [];

    /// <exception cref="ArgumentException">One sound is assigned to two different paths.</exception>
    public SoundConfiguration(IReadOnlyDictionary<SoundSlot, SoundAssignment> assignments)
    {
        foreach (var slot in assignments.Keys)
        {
            if (!Enum.IsDefined(slot))
            {
                throw new ArgumentOutOfRangeException(nameof(assignments), slot, "Unknown sound slot.");
            }
        }

        this.assignments = new Dictionary<SoundSlot, SoundAssignment>(assignments);

        foreach (var (id, path) in assignments.Values)
        {
            if (paths.TryGetValue(id, out var existing) && existing != path)
            {
                throw new ArgumentException(
                    $"Sound '{id.Value}' is assigned to both '{existing}' and '{path}'.", nameof(assignments));
            }

            paths[id] = path;
        }
    }

    /// <summary>The build's assignments from <see cref="SoundAssignments"/>.</summary>
    public static SoundConfiguration Default { get; } = new(SoundAssignments.Slots);

    /// <summary>Returns the command for <paramref name="slot"/>, or <c>null</c> if the slot is unassigned.</summary>
    public PlaySoundCommand? Resolve(SoundSlot slot) =>
        assignments.TryGetValue(slot, out var assignment) ? new PlaySoundCommand(assignment.Id) : null;

    /// <summary>Returns the file path for <paramref name="sound"/>, or <c>null</c> if no slot assigns it.</summary>
    public string? PathOf(SoundId sound) => paths.GetValueOrDefault(sound);
}
