using DrawEM.App.Presentation.Effects;

namespace DrawEM.Tests.Presentation.Effects;

/// <summary>The window messages that keep an effect window click-through and never active.</summary>
public sealed class EffectSurfaceWindowProcedureTests
{
    private const int WM_NCHITTEST = 0x0084;
    private const int WM_MOUSEACTIVATE = 0x0021;
    private const int WM_PAINT = 0x000F;

    [Theory]
    [InlineData(WM_NCHITTEST, -1)]
    [InlineData(WM_MOUSEACTIVATE, 3)]
    public void WindowProcedure_AnswersHitTestAsTransparentAndActivationAsNoActivate(int message, int expected)
    {
        var handled = false;

        var result = EffectSurfaceWindow.WindowProcedure(IntPtr.Zero, message, IntPtr.Zero, IntPtr.Zero, ref handled);

        Assert.True(handled);
        Assert.Equal(new IntPtr(expected), result);
    }

    [Fact]
    public void WindowProcedure_LeavesOtherMessagesToWpf()
    {
        var handled = false;

        var result = EffectSurfaceWindow.WindowProcedure(IntPtr.Zero, WM_PAINT, IntPtr.Zero, IntPtr.Zero, ref handled);

        Assert.False(handled);
        Assert.Equal(IntPtr.Zero, result);
    }
}
