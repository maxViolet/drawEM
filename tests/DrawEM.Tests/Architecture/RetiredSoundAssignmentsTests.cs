using DrawEM.App.Domain.Drawing;
using DrawEM.App.Infrastructure.Sound;
using NetArchTest.Rules;

namespace DrawEM.Tests.Architecture;

[Trait("Category", "Architecture")]
public class RetiredSoundAssignmentsTests
{
    [Fact]
    public void NoProductionType_DependsOnCodeOwnedSoundAssignments()
    {
        var retired = typeof(SoundAssignments).FullName!;

        var result = Types.InAssembly(typeof(DrawingState).Assembly)
            .That().DoNotHaveName(nameof(SoundAssignments))
            .ShouldNot().HaveDependencyOn(retired)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Runtime sounds come from saved settings, not {retired}. " +
            $"Violating types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }
}
