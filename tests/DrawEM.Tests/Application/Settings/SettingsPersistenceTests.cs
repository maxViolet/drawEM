using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Settings;

namespace DrawEM.Tests.Application.Settings;

public class SettingsPersistenceTests
{
    private static readonly SoundReference Applause = new("applause.wav", "Applause.wav");
    private static readonly SoundReference Drumroll = new("drumroll.mp3", "Drumroll.mp3");

    private readonly List<string> calls = [];

    [Fact]
    public void Save_ActivatesAfterStoreWriteAndBeforeMediaRemoval()
    {
        var previous = TestSettings.WithSounds((1, Applause));
        var draft = TestSettings.WithSounds((1, Drumroll));
        var failure = new SoundCleanupFailure(Applause.LibraryFileName, "locked");

        var failures = SettingsPersistence.Save(
            new RecordingStore(calls),
            new RecordingLibrary(calls, [failure]),
            previous,
            draft,
            snapshot =>
            {
                calls.Add(snapshot == draft ? "activate draft" : "activate other");
                return true;
            });

        Assert.Equal(["store draft", "activate draft", "commit previous->draft"], calls);
        Assert.Equal([failure], failures);
    }

    [Fact]
    public void Save_WhenActivationCannotConfirmMediaIsReleased_EndsDraftWithoutRemovingCopies()
    {
        var draft = TestSettings.WithSounds((1, Drumroll));

        var failures = SettingsPersistence.Save(
            new RecordingStore(calls),
            new RecordingLibrary(calls, [new SoundCleanupFailure(Applause.LibraryFileName, "locked")]),
            TestSettings.WithSounds((1, Applause)),
            draft,
            snapshot =>
            {
                calls.Add(snapshot == draft ? "activate draft" : "activate other");
                return false;
            });

        Assert.Equal(["store draft", "activate draft", "keep copies"], calls);
        Assert.Empty(failures);
    }

    [Fact]
    public void Save_WhenStoreFails_PropagatesWithoutActivatingOrRetiringMedia()
    {
        var thrown = new SettingsStoreException("disk full");

        var failure = Assert.Throws<SettingsStoreException>(() => SettingsPersistence.Save(
            new RecordingStore(calls, thrown),
            new RecordingLibrary(calls, []),
            TestSettings.WithSounds((1, Applause)),
            TestSettings.WithSounds((1, Drumroll)),
            _ =>
            {
                calls.Add("activate");
                return true;
            }));

        Assert.Same(thrown, failure);
        Assert.Equal(["store draft"], calls);
    }

    [Fact]
    public void Save_WhenActivationFails_PropagatesWithoutRetiringMedia()
    {
        var thrown = new InvalidOperationException("activation failed");

        var failure = Assert.Throws<InvalidOperationException>(() => SettingsPersistence.Save(
            new RecordingStore(calls),
            new RecordingLibrary(calls, []),
            TestSettings.WithSounds((1, Applause)),
            TestSettings.WithSounds((1, Drumroll)),
            _ => throw thrown));

        Assert.Same(thrown, failure);
        Assert.Equal(["store draft"], calls);
    }

    private static string Name(SettingsSnapshot snapshot) =>
        snapshot.Slots.Select(slot => slot.Action).OfType<SoundAction>().Any(action => action.Sound == Applause)
            ? "previous"
            : "draft";

    private sealed class RecordingStore(List<string> calls, SettingsStoreException? failure = null) : ISettingsStore
    {
        public SettingsLoadResult Load() => throw new NotSupportedException();

        public void Save(SettingsSnapshot snapshot)
        {
            calls.Add("store " + Name(snapshot));
            if (failure is not null)
            {
                throw failure;
            }
        }
    }

    private sealed class RecordingLibrary(List<string> calls, IReadOnlyList<SoundCleanupFailure> failures) : ISoundLibrary
    {
        public SoundReference Import(string sourceFile) => throw new NotSupportedException();

        public IReadOnlyList<SoundCleanupFailure> DiscardDraft(SettingsSnapshot saved) => throw new NotSupportedException();

        public IReadOnlyList<SoundCleanupFailure> CommitSave(SettingsSnapshot previous, SettingsSnapshot saved)
        {
            calls.Add($"commit {Name(previous)}->{Name(saved)}");
            return failures;
        }

        public void CommitSaveKeepingCopies() => calls.Add("keep copies");

        public IReadOnlyList<SoundCleanupFailure> RemoveOrphans(SettingsSnapshot saved) => throw new NotSupportedException();
    }
}
