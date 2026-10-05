using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;
using DrawEM.App.Presentation.Settings;

namespace DrawEM.Tests.Application.Settings;

internal sealed class FakeSettingsSave : ISettingsSave
{
    public List<SettingsSnapshot> Saved { get; } = [];

    /// <summary>Returned instead of saving a valid draft, when set.</summary>
    public SettingsSaveResult? Result { get; set; }

    public SettingsSaveResult Save(DrawingStyle style, Shortcut? draw, Shortcut? clear, IEnumerable<ActionSlot> slots)
    {
        var validation = SettingsSnapshot.Validate(style, draw, clear, slots);
        if (validation.Snapshot is not { } snapshot)
        {
            return new SettingsSaveResult.Invalid(validation.Errors);
        }

        if (Result is not null)
        {
            return Result;
        }

        Saved.Add(snapshot);
        return new SettingsSaveResult.Saved(snapshot);
    }
}

internal sealed class FakeSampler : ISoundSampler
{
    public List<(SoundReference Sound, Action<string> Report)> Samples { get; } = [];

    public void Sample(SoundReference sound, Action<string> reportFailure) => Samples.Add((sound, reportFailure));

    /// <summary>What <see cref="StopSamples"/> returns.</summary>
    public bool StopConfirmed { get; set; } = true;

    public int Stops { get; private set; }

    public bool StopSamples()
    {
        Stops++;
        return StopConfirmed;
    }

    public int Forgets { get; private set; }

    public void ForgetSamples() => Forgets++;
}

internal sealed class FakeCapture : IShortcutCapture
{
    public Action<ShortcutCaptureResult>? Current { get; private set; }

    public int Ended { get; private set; }

    public void Begin(Action<ShortcutCaptureResult> report) => Current = report;

    public void End()
    {
        Current = null;
        Ended++;
    }

    /// <summary>Delivers a result as the hook would, ending capture unless it was rejected.</summary>
    public void Report(ShortcutCaptureResult result)
    {
        var report = Current!;
        if (result is not ShortcutCaptureResult.Rejected)
        {
            Current = null;
        }

        report(result);
    }
}

internal sealed class FakePicker : ISoundFilePicker
{
    public string? Next { get; set; }

    public string? Pick() => Next;
}

internal sealed class FakeLibrary : ISoundLibrary
{
    public List<string> Imported { get; } = [];

    /// <summary>What every import returns.</summary>
    public SoundReference Imports { get; set; } = new("horn.wav", "Horn.wav");

    public List<SettingsSnapshot> Discarded { get; } = [];

    public string? Failure { get; set; }

    public SoundReference Import(string sourceFile)
    {
        if (Failure is not null)
        {
            throw new SoundImportException(Failure);
        }

        Imported.Add(sourceFile);
        return Imports;
    }

    public IReadOnlyList<SoundCleanupFailure> DiscardDraft(SettingsSnapshot saved)
    {
        Discarded.Add(saved);
        return [];
    }

    public IReadOnlyList<SoundCleanupFailure> CommitSave(
        SettingsSnapshot previous, SettingsSnapshot saved, bool removeUnreferencedCopies) => [];

    public IReadOnlyList<SoundCleanupFailure> RemoveOrphans(SettingsSnapshot saved) => [];
}
