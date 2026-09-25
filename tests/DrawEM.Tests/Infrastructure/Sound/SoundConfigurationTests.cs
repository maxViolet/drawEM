using DrawEM.App.Application.Sound;
using DrawEM.App.Infrastructure.Sound;

namespace DrawEM.Tests.Infrastructure.Sound;

public class SoundConfigurationTests
{
    private static readonly SoundId Applause = new("applause");
    private const string ApplausePath = @"C:\Sounds\applause.mp3";

    [Fact]
    public void AssignedSlot_ResolvesToOneCommandForItsSound()
    {
        var configuration = new SoundConfiguration(new Dictionary<SoundSlot, SoundAssignment>
        {
            [SoundSlot.Slot3] = new(Applause, ApplausePath),
        });

        var command = configuration.Resolve(SoundSlot.Slot3);

        Assert.Equal(new PlaySoundCommand(Applause), command);
    }

    [Fact]
    public void UnassignedSlot_ResolvesToNoCommand()
    {
        var configuration = new SoundConfiguration(new Dictionary<SoundSlot, SoundAssignment>
        {
            [SoundSlot.Slot3] = new(Applause, ApplausePath),
        });

        Assert.Null(configuration.Resolve(SoundSlot.Slot4));
    }

    [Fact]
    public void AssignedSound_HasItsConfiguredPath()
    {
        var configuration = new SoundConfiguration(new Dictionary<SoundSlot, SoundAssignment>
        {
            [SoundSlot.Slot3] = new(Applause, ApplausePath),
        });

        Assert.Equal(ApplausePath, configuration.PathOf(Applause));
    }

    [Fact]
    public void UnknownSound_HasNoPath()
    {
        var configuration = new SoundConfiguration(new Dictionary<SoundSlot, SoundAssignment>());

        Assert.Null(configuration.PathOf(Applause));
    }

    [Fact]
    public void SameSoundInTwoSlots_WithSamePath_IsAllowed()
    {
        var configuration = new SoundConfiguration(new Dictionary<SoundSlot, SoundAssignment>
        {
            [SoundSlot.Slot1] = new(Applause, ApplausePath),
            [SoundSlot.Slot2] = new(Applause, ApplausePath),
        });

        Assert.Equal(configuration.Resolve(SoundSlot.Slot1), configuration.Resolve(SoundSlot.Slot2));
    }

    [Fact]
    public void SameSoundInTwoSlots_WithDifferentPaths_IsRejected()
    {
        var assignments = new Dictionary<SoundSlot, SoundAssignment>
        {
            [SoundSlot.Slot1] = new(Applause, ApplausePath),
            [SoundSlot.Slot2] = new(Applause, @"C:\Sounds\other.wav"),
        };

        Assert.Throws<ArgumentException>(() => new SoundConfiguration(assignments));
    }

    [Fact]
    public void SlotOutsideTheEightSoundShortcuts_IsRejected()
    {
        var assignments = new Dictionary<SoundSlot, SoundAssignment>
        {
            [(SoundSlot)9] = new(Applause, ApplausePath),
        };

        Assert.Throws<ArgumentOutOfRangeException>(() => new SoundConfiguration(assignments));
    }
}
