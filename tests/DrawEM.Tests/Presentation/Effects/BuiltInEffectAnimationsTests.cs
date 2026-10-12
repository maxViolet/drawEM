using System.IO;
using System.Text.RegularExpressions;
using DrawEM.App.Application.Effects;
using DrawEM.App.Domain.Effects;
using DrawEM.App.Presentation.Effects;

namespace DrawEM.Tests.Presentation.Effects;

public class BuiltInEffectAnimationsTests
{
    /// <summary>The longest each built-in effect may run, from the roadmap's effect table.</summary>
    private static readonly Dictionary<string, double> SpecifiedSeconds = new()
    {
        ["confetti"] = 3,
        ["focus-ring"] = 2,
    };

    public static TheoryData<string> BuiltInIds => new(EffectCatalog.BuiltIn.Select(effect => effect.Id.Value));

    [Fact]
    public void SpecifiedLengths_CoverEveryBuiltInEffect()
    {
        Assert.Equal(SpecifiedSeconds.Keys.Order(), EffectCatalog.BuiltIn.Select(effect => effect.Id.Value).Order());
    }

    [Theory]
    [MemberData(nameof(BuiltInIds))]
    public void Load_ParsesTheEmbeddedFileWithinItsSpecifiedLengthAndTheChannelDeadline(string id)
    {
        Assert.True(EffectCatalog.TryFind(new EffectId(id), out var effect));

        using var animation = BuiltInEffectAnimations.Load(effect);

        Assert.True(animation.Size.Width > 0 && animation.Size.Height > 0);
        Assert.True(animation.Duration > TimeSpan.Zero);
        Assert.True(animation.Duration.TotalSeconds <= SpecifiedSeconds[id],
            $"{id} lasts {animation.Duration.TotalSeconds} s; at most {SpecifiedSeconds[id]} s is allowed.");
        Assert.True(animation.Duration <= EffectChannelController.MaxDuration);
    }

    [Fact]
    public void Constructor_LoadsEveryBuiltInEffect()
    {
        using var animations = new BuiltInEffectAnimations();

        foreach (var effect in EffectCatalog.BuiltIn)
        {
            Assert.True(animations.TryGet(effect.Id, out var animation, out var failure), failure);
            Assert.NotNull(animation);
        }
    }

    [Fact]
    public void TryGet_ReportsAnEffectOutsideTheCatalog()
    {
        using var animations = new BuiltInEffectAnimations();

        Assert.False(animations.TryGet(new EffectId("sparkles"), out var animation, out var failure));
        Assert.Null(animation);
        Assert.Contains("sparkles", failure);
    }

    [Theory]
    [MemberData(nameof(BuiltInIds))]
    public void Sources_RecordSourceAuthorAndLicenseOfEveryFile(string id)
    {
        Assert.True(EffectCatalog.TryFind(new EffectId(id), out var effect));
        var sources = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "BuiltInEffects", "SOURCES.md"));

        var section = Regex.Match(
            sources, $@"^## {Regex.Escape(effect.ResourceName)}\s*$(?<body>.*?)(?=^## |\z)",
            RegexOptions.Multiline | RegexOptions.Singleline);

        Assert.True(section.Success, $"SOURCES.md has no section for {effect.ResourceName}.");
        var body = section.Groups["body"].Value;
        Assert.Matches(@"(?m)^- Source: https://\S+", body);
        Assert.Matches(@"(?m)^- Author: \S", body);
        Assert.Matches(@"(?m)^- License: \[[^\]]+\]\(https://\S+\)", body);
    }
}
