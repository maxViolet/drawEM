using DrawEM.App.Application.Sound;

namespace DrawEM.App.Infrastructure.Sound;

/// <summary>
/// Code-owned sound assignments for <c>Ctrl+Alt+1</c> through <c>Ctrl+Alt+8</c>.
/// </summary>
/// <remarks>
/// Edit <see cref="Slots"/> before building to assign local sounds. Each entry names a sound and
/// the absolute path to an external WAV or MP3 file on the machine that runs drawEM. The file is
/// not copied into the build; it must exist at that path at run time. Leave a slot out to keep it
/// unassigned. <c>Ctrl+Alt+Z</c> and <c>Ctrl+Alt+X</c> are drawing shortcuts and are not configured here.
/// </remarks>
/// <example>
/// <code>
/// [SoundSlot.Slot1] = new(new SoundId("applause"), @"C:\Users\me\Music\applause.wav"),
/// [SoundSlot.Slot2] = new(new SoundId("drumroll"), @"C:\Users\me\Music\drumroll.mp3"),
/// </code>
/// </example>
public static class SoundAssignments
{
    public static IReadOnlyDictionary<SoundSlot, SoundAssignment> Slots { get; } =
        new Dictionary<SoundSlot, SoundAssignment>
        {
        };
}
