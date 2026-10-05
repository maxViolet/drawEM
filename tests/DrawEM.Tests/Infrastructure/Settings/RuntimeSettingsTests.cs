using System.Text;
using DrawEM.App.Application.Settings;
using DrawEM.App.Application.Sound;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Infrastructure;
using DrawEM.App.Infrastructure.Drawing;
using DrawEM.App.Infrastructure.Settings;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Settings;

/// <summary>Startup composition as in <c>App.xaml.cs</c>: saved settings to bindings, playback, and failure records.</summary>
public sealed class RuntimeSettingsTests : IDisposable
{
    private const ShortcutModifiers CtrlAlt = ShortcutModifiers.Control | ShortcutModifiers.Alt;
    private readonly string root = Path.Combine(Path.GetTempPath(), "drawEM-tests", Guid.NewGuid().ToString("N"));
    private readonly ManualTimeProvider time = new();

    private string SettingsDirectory => Path.Combine(root, "profile");

    private string LibraryDirectory => Path.Combine(SettingsDirectory, "sounds");

    public void Dispose()
    {
        if (Directory.Exists(root))
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void FirstLaunch_BindsDrawAndClearOnly_WithEightEmptySlots()
    {
        var (startup, settings) = Start();

        var bindings = Bind(settings);

        Assert.False(startup.SavedSettingsLoaded);
        Assert.Equal(ActionSlot.Count, settings.Current.Slots.Count);
        Assert.All(settings.Current.Slots, slot => Assert.True(slot.IsEmpty));
        Assert.Equal(new KeyChord(CtrlAlt, VirtualKeys.Z), bindings.Draw);
        Assert.Equal(new KeyChord(CtrlAlt, VirtualKeys.X), bindings.Clear);
        Assert.Empty(bindings.Sounds);
    }

    [Fact]
    public void SavedSoundSlot_BindingResolvesToManagedCopy_AndFailureNamesIt()
    {
        var sound = new ManagedSoundLibrary(LibraryDirectory).Import(WriteSource("applause.wav"));
        new JsonSettingsStore(SettingsDirectory).Save(TestSettings.WithSounds((3, sound)));
        var (_, settings) = Start();
        var binding = Assert.Single(Bind(settings).Sounds);
        var copy = Path.Combine(LibraryDirectory, sound.LibraryFileName);

        Assert.Equal(new KeyChord(CtrlAlt, VirtualKeys.D3), binding.Chord);
        Assert.Equal(copy, binding.Command.Path);
        Assert.True(File.Exists(copy));

        File.Delete(copy);
        var failures = new List<SoundFailure>();
        var reporter = new LoggingSoundFailureReporter(failures.Add, time);
        using (var channel = new SoundChannelController(new MediaSoundPlayerFactory(), reporter, time, action => action()))
        {
            channel.Play(binding.Command);
        }

        reporter.Dispose();
        var failure = Assert.Single(failures);
        Assert.Equal(copy, failure.Path);
        Assert.Equal("File not found.", failure.Reason);
    }

    [Fact]
    public void SnapshotBinding_PlaybackStopsTenSecondsAfterStart()
    {
        var sound = new ManagedSoundLibrary(LibraryDirectory).Import(WriteSource("applause.wav"));
        new JsonSettingsStore(SettingsDirectory).Save(TestSettings.WithSounds((1, sound)));
        var (_, settings) = Start();
        var binding = Assert.Single(Bind(settings).Sounds);
        var players = new PlayerFactory();
        using var channel = new SoundChannelController(players, new NoFailures(), time, action => action());

        channel.Play(binding.Command);
        time.Advance(SoundChannelController.MaxDuration - TimeSpan.FromMilliseconds(1));

        var player = Assert.Single(players.Created);
        Assert.Equal(Path.Combine(LibraryDirectory, sound.LibraryFileName), player.Path);
        Assert.Equal(["play"], player.Calls);

        time.Advance(TimeSpan.FromMilliseconds(1));

        Assert.Equal(["play", "stop", "dispose"], player.Calls);
        Assert.Null(channel.ActiveSound);
    }

    private (StartupSettings Startup, ActiveSettings Settings) Start()
    {
        var startup = SettingsStartup.Load(new JsonSettingsStore(SettingsDirectory, time), new NoSettingsFailures());
        var library = new ManagedSoundLibrary(LibraryDirectory);
        Assert.Empty(SettingsStartup.RemoveOrphanSounds(startup, library));
        return (startup, new ActiveSettings(startup.Active, sound => library.PathFor(sound.LibraryFileName)));
    }

    private static ShortcutBindings Bind(ActiveSettings settings) =>
        ShortcutBindings.FromSnapshot(settings.Current, settings.CommandFor);

    private string WriteSource(string name)
    {
        var source = Path.Combine(root, "source", name);
        Directory.CreateDirectory(Path.GetDirectoryName(source)!);
        File.WriteAllBytes(source, Encoding.UTF8.GetBytes("sound-bytes"));
        return source;
    }

    private sealed class NoSettingsFailures : ISettingsFailureReporter
    {
        public void SettingsUnreadable(string reason, string? recoveryCopy) =>
            throw new InvalidOperationException(reason);
    }

    private sealed class NoFailures : ISoundFailureReporter
    {
        public void Report(PlaySoundCommand command, string reason) =>
            throw new InvalidOperationException(reason);
    }

    /// <summary>Takes the copy from the command like <see cref="MediaSoundPlayerFactory"/>, without opening a media engine.</summary>
    private sealed class PlayerFactory : ISoundPlayerFactory
    {
        public List<FakePlayer> Created { get; } = [];

        public ISoundPlayer Create(PlaySoundCommand command)
        {
            var player = new FakePlayer(command.Path);
            Created.Add(player);
            return player;
        }
    }

    private sealed class FakePlayer(string path) : ISoundPlayer
    {
        public event Action? Completed { add { } remove { } }

        public event Action<string>? Failed { add { } remove { } }

        public string Path { get; } = path;

        public List<string> Calls { get; } = [];

        public void Play() => Calls.Add("play");

        public void Stop() => Calls.Add("stop");

        public void Dispose() => Calls.Add("dispose");
    }
}
