using System.Drawing;
using DrawEM.App.Infrastructure;

namespace DrawEM.Tests.Infrastructure;

public class AppIconTests
{
    [Theory]
    [InlineData(16)]
    [InlineData(20)]
    [InlineData(24)]
    [InlineData(32)]
    [InlineData(48)]
    public void IconResource_HasAFrameForEachTraySize(int size)
    {
        using var stream = System.Windows.Application.GetResourceStream(NotifyIconTrayHost.IconUri).Stream;
        using var icon = new Icon(stream, size, size);

        Assert.Equal(new Size(size, size), icon.Size);
        using var bitmap = icon.ToBitmap();
        Assert.Equal(255, bitmap.GetPixel(size / 2, size / 2).A);
    }
}
