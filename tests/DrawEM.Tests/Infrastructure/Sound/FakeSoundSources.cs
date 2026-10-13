using DrawEM.App.Application.Sound;

namespace DrawEM.Tests.Infrastructure.Sound;

internal sealed class RecordingSoundFailures(List<string> reasons) : ISoundFailureReporter
{
    public void Report(PlaySoundCommand command, string reason) => reasons.Add(reason);
}

/// <summary>Records players without opening a media engine.</summary>
internal sealed class FakePlayerFactory : ISoundPlayerFactory
{
    public List<FakePlayer> Created { get; } = [];

    public Action? OnStop { get; set; }

    public ISoundPlayer Create(PlaySoundCommand command)
    {
        var player = new FakePlayer(command.Sound, () => OnStop?.Invoke());
        Created.Add(player);
        return player;
    }
}

internal sealed class FakePlayer(SoundId sound, Action onStop) : ISoundPlayer
{
    public event Action? Completed { add { } remove { } }

    public event Action<string>? Failed { add { } remove { } }

    public SoundId Sound { get; } = sound;

    public List<string> Calls { get; } = [];

    public void Play() => Calls.Add("play");

    public void Stop()
    {
        Calls.Add("stop");
        onStop();
    }

    public void Dispose() => Calls.Add("dispose");
}
